using Microsoft.EntityFrameworkCore;
using TaleLearnCode.ThumbnailGenerator.Data;
using TaleLearnCode.ThumbnailGenerator.Domain;
using TaleLearnCode.ThumbnailGenerator.Models;

namespace TaleLearnCode.ThumbnailGenerator.Extensions;

internal static class EventSubscriptionNotificationExtensions
{

	private const int _subjectContainerIndex = 4;
	private const string _blobs = "/blobs/";

	internal static string GetStorageResourceName(this EventSubscriptionNotification eventSubscriptionNotification)
		=> eventSubscriptionNotification.Topic.Split('/').Last();

	internal static string GetStorageContainerName(this EventSubscriptionNotification eventSubscriptionNotification)
		=> eventSubscriptionNotification.Subject.Split('/')[_subjectContainerIndex];

	internal static string GetBlobName(this EventSubscriptionNotification eventSubscriptionNotification)
		=> eventSubscriptionNotification.Subject[(eventSubscriptionNotification.Subject.IndexOf(_blobs) + _blobs.Length)..];

	internal static async Task<bool> IsTemplateSpecificContainerAsync(this EventSubscriptionNotification eventSubscriptionNotification, ThumbnailGeneratorContext thumbnailGeneratorContext)
	{
		string storageResourceName = GetStorageContainerName(eventSubscriptionNotification);
		string storageContainerName = GetStorageContainerName(eventSubscriptionNotification);
		StorageAccount? storageAccount = await thumbnailGeneratorContext.StorageAccounts.FirstOrDefaultAsync(x => x.ResourceName == storageResourceName && x.ContainerName == storageContainerName);
		if (storageAccount is null)
		{
			Tenant? tenant = await thumbnailGeneratorContext.Tenants.FirstOrDefaultAsync(x => x.TenantKey == storageContainerName);
			return (tenant is not null);
		}
		return false;
	}

}