namespace TaleLearnCode.ThumbnailGenerator.Domain;

public class ThumbnailGeneratorConfig
{
	public int ThumbnailGeneratorConfigId { get; set; }
	public string ThumbnailGeneratorConfigName { get; set; }
	public int OriginStorageAccountId { get; set; }
	public int ThumbnailStorageAccountId { get; set; }
	public int ThumbnailHeight { get; set; }
	public int ThumbnailWidth { get; set; }
	public string ThumbnailPrefix { get; set; }
	public string ThumbnailSuffix { get; set; }
	public virtual StorageAccount OriginStorageAccount { get; set; }
	public virtual StorageAccount ThumbnailStorageAccount { get; set; }
}