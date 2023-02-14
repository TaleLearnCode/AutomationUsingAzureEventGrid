#nullable disable

namespace TaleLearnCode.ThumbnailGenerator.Models;

public class GenerateThumbnailRequest
{
	public string StorageAccountName { get; set; }
	public string ContainerName { get; set; }
	public string BlobName { get; set; }
	public bool IsTenantSpecific { get; set; }
	public bool OverwriteIfExists { get; set; }
}