// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
// http://localhost:7150/runtime/webhooks/EventGrid?functionName=ThumbnailGeneratorEventGrid
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TaleLearnCode.ThumbnailGenerator.Exceptions;
using TaleLearnCode.ThumbnailGenerator.Models;

namespace TaleLearnCode.ThumbnailGenerator.Functions;

public class ThumbnailGeneratorEventGrid
{

	private readonly ILogger _logger;
	private readonly IConfiguration _configuration;

	private const string _triggerType = "EventGrid";

	public ThumbnailGeneratorEventGrid(ILoggerFactory loggerFactory, IConfiguration configuration)
	{
		_logger = loggerFactory.CreateLogger<ThumbnailGeneratorEventGrid>();
		_configuration = configuration;
	}

	[Function("ThumbnailGeneratorEventGrid")]
	public async Task RunAsync([EventGridTrigger] EventSubscriptionNotification notification)
	{
		try
		{
			if (notification is not null)
			{
				Services services = new(_configuration);
				switch (notification.EventType)
				{
					case "Microsoft.Storage.BlobCreated":
						await services.GenerateThumbnailAsync(notification, _triggerType);
						break;
					case "Microsoft.Storage.BlobDeleted":
						await services.DeleteThumbnailAsync(notification, _triggerType);
						break;
				}
			}
		}
		catch (ThumbnailGeneratorException ex)
		{
			_logger.LogError("{ExceptionMessage}", ex.Message);
		}
		catch (Exception ex)
		{
			_logger.LogError("Unexpected exception: {ExceptionMessage}", ex.Message);
			throw;
		}
	}

}