#nullable disable

namespace TaleLearnCode.ThumbnailGenerator.Models;

public class ImageDetailResponse
{
	public string StorageAccountName { get; set; }
	public string ContainerName { get; set; }
	public string BlobName { get; set; }
	public string BlobUrl { get; set; }
	public long? ContentLength { get; set; }
	public int Height { get; set; }
	public int Width { get; set; }
}