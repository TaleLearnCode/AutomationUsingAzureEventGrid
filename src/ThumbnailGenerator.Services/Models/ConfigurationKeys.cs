namespace TaleLearnCode.ThumbnailGenerator.Models;

internal static class ConfigurationKeys
{

	private const string _storageConnectionStrings = "ThumbnailGenerator:StorageConnectionStrings:";


	internal const string DatabaseConnectionString = "DatabaseConnectionString";
	internal const string DefaultThumbnailWidth = "ThumbnailGenerator:DefaultThumbnailWidth";
	internal const string DefaultThumbnailHeight = "ThumbnailGenerator:DefaultThumbnailHeight";
	internal const string LogActivity = "ThumbnailGenerator:LogActivity";
	internal const string LogConnectionString = "ThumbnailGenerator:LogConnectionString";
	internal const string LogTableName = "ThumbnailGenerator:LogTableName";

	internal static string StorageConnectionStrings(string storageAccountName)
		=> $"{_storageConnectionStrings}{storageAccountName}";

}