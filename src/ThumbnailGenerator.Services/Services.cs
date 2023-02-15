
using Azure;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using TaleLearnCode.ThumbnailGenerator.Extensions;

namespace TaleLearnCode.ThumbnailGenerator;

public class Services
{

	private readonly IConfigurationRoot _config;

	private const string _tenantSpecificContainerName = "TenantSpecific";

	private const string _createOperation = "Create";
	private const string _deleteOperation = "Delete";

	public Services(IConfigurationRoot config) => _config = config;

	public async Task<GenerateThumbnailResponse> GenerateThumbnailAsync(
		GenerateThumbnailRequest request,
		string triggerType)
	{
		await using ThumbnailGeneratorContext thumbnailGeneratorContext = new(_config[ConfigurationKeys.DatabaseConnectionString]);
		return await GenerateThumbnailAsync(thumbnailGeneratorContext, request, Guid.NewGuid().ToString(), triggerType);
	}

	public async Task<GenerateThumbnailResponse> GenerateThumbnailAsync(
		EventSubscriptionNotification eventSubscriptionNotification,
		string triggerType)
	{
		await using ThumbnailGeneratorContext thumbnailGeneratorContext = new(_config[ConfigurationKeys.DatabaseConnectionString]);
		return await GenerateThumbnailAsync(
			thumbnailGeneratorContext,
			new()
			{
				StorageAccountName = eventSubscriptionNotification.GetStorageResourceName(),
				ContainerName = eventSubscriptionNotification.GetStorageContainerName(),
				BlobName = eventSubscriptionNotification.GetBlobName(),
				IsTenantSpecific = await eventSubscriptionNotification.IsTemplateSpecificContainerAsync(thumbnailGeneratorContext),
				OverwriteIfExists = false
			},
		eventSubscriptionNotification.Data.RequestId,
		triggerType);
	}

	public async Task<GenerateThumbnailResponse> DeleteThumbnailAsync(
		EventSubscriptionNotification eventSubscriptionNotification,
		string triggerType)
	{
		await using ThumbnailGeneratorContext thumbnailGeneratorContext = new(_config[ConfigurationKeys.DatabaseConnectionString]);
		return await DeleteThumbnailAsync(
			thumbnailGeneratorContext,
			new()
			{
				StorageAccountName = eventSubscriptionNotification.GetStorageResourceName(),
				ContainerName = eventSubscriptionNotification.GetStorageContainerName(),
				BlobName = eventSubscriptionNotification.GetBlobName(),
				IsTenantSpecific = await eventSubscriptionNotification.IsTemplateSpecificContainerAsync(thumbnailGeneratorContext),
				OverwriteIfExists = false
			},
			eventSubscriptionNotification.Data.RequestId,
			triggerType);
	}

	private async Task<GenerateThumbnailResponse> GenerateThumbnailAsync(
		ThumbnailGeneratorContext thumbnailGeneratorContext,
		GenerateThumbnailRequest generateThumbnailRequest,
		string operationId,
		string triggerType)
	{

		GenerateThumbnailResponse? response = default;
		Stopwatch stopwatch = Stopwatch.StartNew();
		ThumbnailGeneratorConfig? thumbnailGeneratorConfig = default;
		string? originContainerName = default;

		try
		{

			string blobNameWithoutPath = GetBlobNameWithoutPath(generateThumbnailRequest);

			thumbnailGeneratorConfig = await GetThumbnailGeneratorConfigAsync(thumbnailGeneratorContext, generateThumbnailRequest, operationId);

			originContainerName = (thumbnailGeneratorConfig.OriginStorageAccount.ContainerName != _tenantSpecificContainerName)
				? thumbnailGeneratorConfig.OriginStorageAccount.ContainerName
				: generateThumbnailRequest.ContainerName;
			string thumbnailContainerName = (thumbnailGeneratorConfig.ThumbnailStorageAccount.ContainerName != _tenantSpecificContainerName)
				? thumbnailGeneratorConfig.ThumbnailStorageAccount.ContainerName
				: generateThumbnailRequest.ContainerName;

			string thumbnailStorageConnectionString = GetStorageConnectionString(thumbnailGeneratorConfig.ThumbnailStorageAccount.ResourceName, operationId);

			BlobClient originBlobClient = GetBlobClient(
				thumbnailGeneratorConfig.OriginStorageAccount.ResourceName,
				originContainerName,
				generateThumbnailRequest.BlobName,
				operationId);
			Stream originBlobStream = await RetrieveOriginBlob(originBlobClient, operationId);

			IImageEncoder encoder = GetEncoder(generateThumbnailRequest.BlobName, operationId);

			BlobContainerClient blobContainerClient = new BlobServiceClient(thumbnailStorageConnectionString).GetBlobContainerClient(thumbnailContainerName);

			using MemoryStream output = new();
			using Image image = Image.Load(originBlobStream);
			ImageDimensions thumbnailDimensions = new(image, thumbnailGeneratorConfig, _config.GetIntValue(ConfigurationKeys.DefaultThumbnailWidth), _config.GetIntValue(ConfigurationKeys.DefaultThumbnailHeight));
			image.Mutate(x => x.Resize(thumbnailDimensions.ThumbnailWidth, thumbnailDimensions.ThumbnailHeight));
			image.Save(output, encoder);
			output.Position = 0;

			string thumbnailName = await SaveThumbnailAsync(blobNameWithoutPath, thumbnailGeneratorConfig, blobContainerClient, output, generateThumbnailRequest.OverwriteIfExists, operationId);

			BlobClient thumbnailBlobClient = GetBlobClient(thumbnailGeneratorConfig.ThumbnailStorageAccount.ResourceName, thumbnailContainerName, thumbnailName, operationId);

			response = await GenerateResponseAsync(generateThumbnailRequest, operationId, stopwatch, HttpStatusCode.Created, thumbnailGeneratorConfig, originContainerName, thumbnailContainerName, originBlobClient, thumbnailDimensions, thumbnailName, thumbnailBlobClient);

		}
		catch (OriginNotConfiguredException ex)
		{
			response = GenerateResponse(generateThumbnailRequest, operationId, stopwatch, ex.Status, thumbnailGeneratorConfig, originContainerName, ex);
			throw;
		}
		catch (ThumbnailGeneratorException ex)
		{
			response = GenerateResponse(generateThumbnailRequest, operationId, stopwatch, ex.Status, thumbnailGeneratorConfig, originContainerName, ex);
			throw;
		}
		catch (Exception ex)
		{
			response = GenerateResponse(generateThumbnailRequest, operationId, stopwatch, HttpStatusCode.InternalServerError, thumbnailGeneratorConfig, originContainerName, ex);
			throw new ThumbnailGeneratorException($"Unexpected exception: {ex.Message}", ex, operationId, HttpStatusCode.InternalServerError);
		}
		finally
		{
			await RecordOperationLogAsync(response, triggerType, _createOperation);
		}

		return response;
	}

	private async Task<GenerateThumbnailResponse> DeleteThumbnailAsync(
		ThumbnailGeneratorContext thumbnailGeneratorContext,
		GenerateThumbnailRequest generateThumbnailRequest,
		string operationId,
		string triggerType)
	{

		GenerateThumbnailResponse? response = default;
		Stopwatch stopwatch = Stopwatch.StartNew();
		ThumbnailGeneratorConfig thumbnailGeneratorConfig = new();
		string? originContainerName = default;
		long thumbnailContentLength = default;
		string? thumbnailContainerName = default;
		string? thumbnailName = default;

		try
		{

			string blobNameWithoutPath = GetBlobNameWithoutPath(generateThumbnailRequest);
			thumbnailGeneratorConfig = await GetThumbnailGeneratorConfigAsync(thumbnailGeneratorContext, generateThumbnailRequest, operationId);
			originContainerName = GetContainerName(thumbnailGeneratorConfig.OriginStorageAccount.ContainerName, generateThumbnailRequest.ContainerName);
			thumbnailContainerName = GetContainerName(thumbnailGeneratorConfig.ThumbnailStorageAccount.ContainerName, generateThumbnailRequest.ContainerName);

			thumbnailName = GenerateThumbnailName(blobNameWithoutPath, thumbnailGeneratorConfig);
			BlobClient thumbnailBlobClient = GetBlobClient(thumbnailGeneratorConfig.ThumbnailStorageAccount.ResourceName, thumbnailContainerName, thumbnailName, operationId);
			if (await thumbnailBlobClient.ExistsAsync()) thumbnailContentLength = await GetContentLengthAsync(thumbnailBlobClient);
			await thumbnailBlobClient.DeleteIfExistsAsync();

			response = GenerateResponse(
				generateThumbnailRequest,
				operationId,
				stopwatch,
				HttpStatusCode.OK,
				thumbnailGeneratorConfig,
				originContainerName,
				thumbnailContainerName,
				thumbnailName,
				thumbnailContentLength);

		}
		catch (OriginBlobNotFoundException)
		{
			response = GenerateResponse(
				generateThumbnailRequest,
				operationId,
				stopwatch,
				HttpStatusCode.OK,
				thumbnailGeneratorConfig,
				originContainerName,
				thumbnailContainerName,
				thumbnailName,
				thumbnailContentLength);
		}
		catch (OriginNotConfiguredException ex)
		{
			response = GenerateResponse(generateThumbnailRequest, operationId, stopwatch, ex.Status, thumbnailGeneratorConfig, originContainerName, ex);
			throw;
		}
		catch (ThumbnailGeneratorException ex)
		{
			response = GenerateResponse(generateThumbnailRequest, operationId, stopwatch, ex.Status, thumbnailGeneratorConfig, originContainerName, ex);
			throw;
		}
		catch (Exception ex)
		{
			response = GenerateResponse(generateThumbnailRequest, operationId, stopwatch, HttpStatusCode.InternalServerError, thumbnailGeneratorConfig, originContainerName, ex);
			throw new ThumbnailGeneratorException($"Unexpected exception: {ex.Message}", ex, operationId, HttpStatusCode.InternalServerError);
		}
		finally
		{
			await RecordOperationLogAsync(response, triggerType, _deleteOperation);
		}
		return response;
	}


	private static string GetBlobNameWithoutPath(GenerateThumbnailRequest generateThumbnailRequest)
		=> (generateThumbnailRequest.BlobName.Contains('/'))
			? generateThumbnailRequest.BlobName[(generateThumbnailRequest.BlobName.LastIndexOf('/') + 1)..]
			: generateThumbnailRequest.BlobName;

	private static async Task<ThumbnailGeneratorConfig> GetThumbnailGeneratorConfigAsync(
		ThumbnailGeneratorContext thumbnailGeneratorContext,
		GenerateThumbnailRequest generateThumbnailRequest,
		string operationId)
	{

		string? originPath = generateThumbnailRequest.BlobName.Contains('/')
			? generateThumbnailRequest.BlobName[..generateThumbnailRequest.BlobName.LastIndexOf('/')]
			: default;

		List<ThumbnailGeneratorConfig> thumbnailGeneratorConfigs = await thumbnailGeneratorContext.ThumbnailGeneratorConfigs
			.Include(x => x.OriginStorageAccount)
			.Include(x => x.ThumbnailStorageAccount)
			.Where(x => x.OriginStorageAccount.ResourceName == generateThumbnailRequest.StorageAccountName)
			.ToListAsync();

		thumbnailGeneratorConfigs = (generateThumbnailRequest.IsTenantSpecific)
			? thumbnailGeneratorConfigs.Where(x => x.OriginStorageAccount.ContainerName == _tenantSpecificContainerName).ToList()
			: thumbnailGeneratorConfigs.Where(x => x.OriginStorageAccount.ContainerName == generateThumbnailRequest.ContainerName).ToList();

		ThumbnailGeneratorConfig? response = thumbnailGeneratorConfigs.FirstOrDefault(x => x.OriginStorageAccount.FolderPath == originPath);
		if (response is not null) return response;

		if (originPath is not null)
		{
			thumbnailGeneratorConfigs = thumbnailGeneratorConfigs.Where(x => x.OriginStorageAccount.AllowSubfolders == true).ToList();
			foreach (ThumbnailGeneratorConfig thumbnailGeneratorConfig in thumbnailGeneratorConfigs)
				if (originPath.StartsWith(thumbnailGeneratorConfig.OriginStorageAccount.FolderPath[..^1]))
					return thumbnailGeneratorConfig;
		}

		throw new OriginNotConfiguredException("No thumbnail generation configuration exists for the origin.", operationId, HttpStatusCode.BadRequest);

	}

	private string GetStorageConnectionString(string storageAccountName, string operationId)
		=> _config[ConfigurationKeys.StorageConnectionStrings(storageAccountName)]
			?? throw new OriginNotConfiguredException($"No storage connection string exists for {storageAccountName}.", operationId, HttpStatusCode.BadRequest);

	private BlobClient GetBlobClient(
		string storageAccountName,
		string blobContainerName,
		string blobName,
		string operationId)
	{
		try
		{
			return new BlobClient(GetStorageConnectionString(storageAccountName, operationId), blobContainerName, blobName);
		}
		catch (RequestFailedException ex) when (ex.ErrorCode == "BlobNotFound")
		{
			throw new OriginBlobNotFoundException($"The '{blobName}' does not exist in the '{blobContainerName}' container.", ex, operationId, HttpStatusCode.NotFound);
		}
	}

	private static async Task<Stream> RetrieveOriginBlob(
		BlobClient blobClient,
		string operationId)
	{
		try
		{
			Stream input = await blobClient.OpenReadAsync();
			return input is null
				? throw new OriginBlobNotFoundException("Unable to load the origin blob.", operationId, HttpStatusCode.NotFound)
				: input;
		}
		catch (RequestFailedException ex) when (ex.ErrorCode == "BlobNotFound")
		{
			throw new OriginBlobNotFoundException("unable to load the origin blob.", operationId, HttpStatusCode.NotFound);
		}
	}

	private static IImageEncoder GetEncoder(string blobName, string operationId)
		=> ((Path.GetExtension(blobName)).Replace(".", "")).ToLower() switch
		{
			"png" => new PngEncoder(),
			"jpg" => new JpegEncoder(),
			"jpeg" => new JpegEncoder(),
			"gif" => new GifEncoder(),
			_ => throw new NoEncoderSupportException($"No encoder support for: {blobName}", operationId, HttpStatusCode.BadRequest)
		};

	private static async Task<string> SaveThumbnailAsync(
		string blobNameWithoutPath,
		ThumbnailGeneratorConfig thumbnailGeneratorConfig,
		BlobContainerClient blobContainerClient,
		MemoryStream output,
		bool overwriteIfExists,
		string operationId)
	{
		string thumbnailName = GenerateThumbnailName(blobNameWithoutPath, thumbnailGeneratorConfig);
		try
		{
			if (overwriteIfExists) await blobContainerClient.DeleteBlobIfExistsAsync(thumbnailName);
			await blobContainerClient.UploadBlobAsync(thumbnailName, output);
			return thumbnailName;
		}
		catch (RequestFailedException ex) when (ex.ErrorCode == "BlobAlreadyExists")
		{
			throw new ThumbnailAlreadyExistsException($"The '{thumbnailName}' already exists in the '{blobContainerClient.Name}' container.", ex, operationId, HttpStatusCode.Conflict);
		}
	}

	private static string GenerateThumbnailName(string blobNameWithoutPath, ThumbnailGeneratorConfig thumbnailGeneratorConfig)
		=> string.Concat(
			thumbnailGeneratorConfig.ThumbnailStorageAccount.FolderPath,
			!thumbnailGeneratorConfig.ThumbnailStorageAccount.FolderPath.EndsWith('/') ? "/" : string.Empty,
			thumbnailGeneratorConfig.ThumbnailPrefix,
			blobNameWithoutPath,
			thumbnailGeneratorConfig.ThumbnailSuffix);

	private static async Task<GenerateThumbnailResponse> GenerateResponseAsync(
		GenerateThumbnailRequest generateThumbnailRequest,
		string operationId,
		Stopwatch stopwatch,
		HttpStatusCode statusCode,
		ThumbnailGeneratorConfig thumbnailGeneratorConfig,
		string originContainerName,
		string thumbnailContainerName,
		BlobClient originBlobClient,
		ImageDimensions thumbnailDimensions,
		string thumbnailName,
		BlobClient thumbnailBlobClient)
	{
		return new()
		{
			OperationId = operationId,
			Status = statusCode,
			ConfigName = thumbnailGeneratorConfig.ThumbnailGeneratorConfigName,
			OperationDuration = stopwatch.ElapsedMilliseconds,
			Origin = new()
			{
				StorageAccountName = thumbnailGeneratorConfig.OriginStorageAccount.ResourceName,
				ContainerName = originContainerName,
				BlobName = generateThumbnailRequest.BlobName,
				BlobUrl = originBlobClient.Uri.ToString(),
				ContentLength = await GetContentLengthAsync(originBlobClient),
				Height = thumbnailDimensions.OriginHeight,
				Width = thumbnailDimensions.OriginWidth
			},
			Thumbnail = new()
			{
				StorageAccountName = thumbnailGeneratorConfig.ThumbnailStorageAccount.ResourceName,
				ContainerName = thumbnailContainerName,
				BlobName = thumbnailName,
				BlobUrl = thumbnailBlobClient.Uri.ToString(),
				ContentLength = await GetContentLengthAsync(thumbnailBlobClient),
				Height = thumbnailDimensions.ThumbnailHeight,
				Width = thumbnailDimensions.ThumbnailWidth
			}
		};
	}

	private static async Task<long> GetContentLengthAsync(BlobClient blobClient)
	{
		BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
		return blobProperties.ContentLength;
	}

	private static GenerateThumbnailResponse GenerateResponse(
		GenerateThumbnailRequest generateThumbnailRequest,
		string operationId,
		Stopwatch stopwatch,
		HttpStatusCode statusCode,
		ThumbnailGeneratorConfig? thumbnailGeneratorConfig,
		string? originContainerName,
		Exception thumbnailGeneratorException)
		=> new()
		{
			OperationId = operationId,
			Status = statusCode,
			ConfigName = thumbnailGeneratorConfig?.ThumbnailGeneratorConfigName ?? "Config Not Found",
			OperationDuration = stopwatch.ElapsedMilliseconds,
			Exception = thumbnailGeneratorException,
			Origin = new()
			{
				StorageAccountName = thumbnailGeneratorConfig?.OriginStorageAccount.ResourceName ?? generateThumbnailRequest.StorageAccountName,
				ContainerName = originContainerName ?? generateThumbnailRequest.ContainerName,
				BlobName = generateThumbnailRequest.BlobName
			}
		};

	private static GenerateThumbnailResponse GenerateResponse(
		GenerateThumbnailRequest generateThumbnailRequest,
		string operationId,
		Stopwatch stopwatch,
		HttpStatusCode statusCode,
		ThumbnailGeneratorConfig? thumbnailGeneratorConfig,
		string? originContainerName,
		string? thumbnailContainerName,
		string? thumbnailName,
		long? thumbnailContentLength)
		=> new()
		{
			OperationId = operationId,
			Status = statusCode,
			ConfigName = thumbnailGeneratorConfig?.ThumbnailGeneratorConfigName,
			OperationDuration = stopwatch.ElapsedMilliseconds,
			Origin = new()
			{
				StorageAccountName = thumbnailGeneratorConfig?.OriginStorageAccount.ResourceName,
				ContainerName = originContainerName,
				BlobName = generateThumbnailRequest.BlobName
			},
			Thumbnail = new()
			{
				StorageAccountName = thumbnailGeneratorConfig?.ThumbnailStorageAccount.ResourceName,
				ContainerName = thumbnailContainerName,
				BlobName = thumbnailName,
				ContentLength = thumbnailContentLength
			}
		};

	private async Task RecordOperationLogAsync(
		GenerateThumbnailResponse? generateThumbnailResponse,
		string triggerType,
		string operationType)
	{

		string logConnectionString = _config[ConfigurationKeys.LogConnectionString];
		string logTableName = _config[ConfigurationKeys.LogTableName];


		if (generateThumbnailResponse is not null && _config.GetBoolValue(ConfigurationKeys.LogActivity) && !string.IsNullOrWhiteSpace(logConnectionString) && !string.IsNullOrWhiteSpace(logTableName))
		{
			TableServiceClient tableServiceClient = new(logConnectionString);
			TableClient tableClient = tableServiceClient.GetTableClient(logTableName);
			await tableClient.CreateIfNotExistsAsync();
			await tableClient.AddEntityAsync(new GenerateThumbnailLogEntry(generateThumbnailResponse, triggerType, operationType));
		}

	}

	private static string GetContainerName(string configuredContainerName, string requestContainerName)
		=> (configuredContainerName != _tenantSpecificContainerName)
		? configuredContainerName
		: requestContainerName;



























}