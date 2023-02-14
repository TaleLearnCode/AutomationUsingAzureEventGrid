using Microsoft.Extensions.Configuration;

namespace TaleLearnCode.ThumbnailGenerator;

public class Services
{

	private readonly IConfigurationRoot _config;
	private readonly int _defaultThumbnailWidth;
	private readonly int _defaultThumbnailHeight;
	private readonly bool _logActivity;
	private readonly string? _logConnectionString;
	private readonly string? _logTableName;

	private const string _tenantSpecificContainerName = "TenantSpecific";

	private const string _createOperation = "Create";
	private const string _deleteOperation = "Delete";

}