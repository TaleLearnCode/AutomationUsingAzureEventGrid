#nullable disable

namespace TaleLearnCode.ThumbnailGenerator.Models;

public class GenerateThumbnailResponse
{
	public string OperationId { get; set; }
	public HttpStatusCode Status { get; set; }
	public string ConfigName { get; set; }
	public long OperationDuration { get; set; }
	public ImageDetailResponse Origin { get; set; }
	public ImageDetailResponse Thumbnail { get; set; }
	public Exception Exception { get; set; }
}