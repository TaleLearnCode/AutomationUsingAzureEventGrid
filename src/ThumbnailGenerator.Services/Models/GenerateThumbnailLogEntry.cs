using Azure;
using Azure.Data.Tables;

#nullable disable

namespace TaleLearnCode.ThumbnailGenerator.Models;

public class GenerateThumbnailLogEntry : ITableEntity
{

	public GenerateThumbnailLogEntry(
		GenerateThumbnailResponse generateThumbnailResponse,
		string triggerType,
		string operationType)
	{
		PartitionKey = generateThumbnailResponse.ConfigName;
		RowKey = generateThumbnailResponse.OperationId;
		OperationId = generateThumbnailResponse.OperationId;
		TriggerType = triggerType;
		OperationType = operationType;
		Status = (int)generateThumbnailResponse.Status;
		ConfigName = generateThumbnailResponse.ConfigName;
		OperationDuration = generateThumbnailResponse.OperationDuration;


		if (generateThumbnailResponse.Origin is not null)
		{
			OriginStorageAccountName = generateThumbnailResponse.Origin.StorageAccountName;
			OriginContainerName = generateThumbnailResponse.Origin.ContainerName;
			OriginBlobName = generateThumbnailResponse.Origin.BlobName;
			OriginBlobUrl = generateThumbnailResponse.Origin.BlobUrl;
			OriginContentLength = generateThumbnailResponse.Origin.ContentLength;
			OriginHeight = generateThumbnailResponse.Origin.Height;
			OriginWidth = generateThumbnailResponse.Origin.Width;
		}

		if (generateThumbnailResponse.Origin is not null)
		{
			ThumbnailStorageAccountName = generateThumbnailResponse.Thumbnail.StorageAccountName;
			ThumbnailContainerName = generateThumbnailResponse.Thumbnail.ContainerName;
			ThumbnailBlobName = generateThumbnailResponse.Thumbnail.BlobName;
			ThumbnailBlobUrl = generateThumbnailResponse.Thumbnail.BlobUrl;
			ThumbnailContentLength = generateThumbnailResponse.Thumbnail.ContentLength;
			ThumbnailHeight = generateThumbnailResponse.Thumbnail.Height;
			ThumbnailWidth = generateThumbnailResponse.Thumbnail.Width;
		}

		if (generateThumbnailResponse.Exception is not null)
		{
			ExceptionType = (generateThumbnailResponse.Exception.GetType().ToString().Split('.')).Last();
			ExceptionMessage = generateThumbnailResponse.Exception.Message;
			if (generateThumbnailResponse.Exception.InnerException is not null)
			{
				ExceptionType = (generateThumbnailResponse.Exception.InnerException.GetType().ToString().Split('.')).Last();
				ExceptionMessage = generateThumbnailResponse.Exception.InnerException.Message;
			}
		}
	}


	public string PartitionKey { get; set; }
	public string RowKey { get; set; }
	public DateTimeOffset? Timestamp { get; set; }
	public ETag ETag { get; set; }

	public string OperationId { get; set; }
	public string TriggerType { get; set; }
	public string OperationType { get; set; }
	public int Status { get; set; }
	public string ConfigName { get; set; }
	public long OperationDuration { get; set; }

	public string OriginStorageAccountName { get; set; }
	public string OriginContainerName { get; set; }
	public string OriginBlobName { get; set; }
	public string OriginBlobUrl { get; set; }
	public long? OriginContentLength { get; set; }
	public int OriginHeight { get; set; }
	public int OriginWidth { get; set; }

	public string ThumbnailStorageAccountName { get; set; }
	public string ThumbnailContainerName { get; set; }
	public string ThumbnailBlobName { get; set; }
	public string ThumbnailBlobUrl { get; set; }
	public long? ThumbnailContentLength { get; set; }
	public int ThumbnailHeight { get; set; }
	public int ThumbnailWidth { get; set; }

	public string ExceptionType { get; set; }
	public string ExceptionMessage { get; set; }
	public string InnerExceptionType { get; set; }
	public string InnerExceptionMessage { get; set; }

}