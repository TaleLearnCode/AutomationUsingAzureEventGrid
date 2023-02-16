using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using TaleLearnCode.ThumbnailGenerator.Exceptions;
using TaleLearnCode.ThumbnailGenerator.Models;

namespace TaleLearnCode.ThumbnailGenerator;

public class ThumbnailGeneratorHttp
{
	private readonly ILogger _logger;
	private readonly IConfiguration _configuration;
	private readonly JsonSerializerOptions _jsonSerializerOptions;

	private const string _triggerType = "Http";

	public ThumbnailGeneratorHttp(
		ILoggerFactory loggerFactory,
		IConfiguration configuration,
		JsonSerializerOptions jsonSerializerOptions)
	{
		_logger = loggerFactory.CreateLogger<ThumbnailGeneratorHttp>();
		_configuration = configuration;
		_jsonSerializerOptions = jsonSerializerOptions;
	}

	[Function("ThumbnailGeneratorHttp")]
	public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request)
	{
		try
		{
			Services services = new(_configuration);
			GenerateThumbnailResponse generateThumbnailResponse =
				await services.GenerateThumbnailAsync(
					await request.GetRequestParametersAsync<GenerateThumbnailRequest>(_jsonSerializerOptions),
					_triggerType);
			_logger.LogInformation("ThumbnailGenerator process complete. Operation Id: {Operation Id}; Config: {ConfigName}; Status: {Status}; Duration: {OperationDuration}", generateThumbnailResponse.OperationId, generateThumbnailResponse.ConfigName, generateThumbnailResponse.Status, generateThumbnailResponse.OperationDuration);
			return await request.CreateResponseAsync(generateThumbnailResponse, _jsonSerializerOptions, generateThumbnailResponse.Status, generateThumbnailResponse.Status);
		}
		catch (ThumbnailGeneratorException ex)
		{
			_logger.LogError("{ExceptionMessage}", ex.Message);
			return await request.CreateResponseAsync(new ExceptionResponse(ex), _jsonSerializerOptions, ex.Status, ex.Status);
		}
		catch (Exception ex)
		{
			_logger.LogError("Unexpected exception: {ExceptionMessage}", ex.Message);
			return await request.CreateResponseAsync(
				new ExceptionResponse(ex, HttpStatusCode.InternalServerError),
				_jsonSerializerOptions,
				HttpStatusCode.InternalServerError,
				HttpStatusCode.InternalServerError);
		}
	}

}