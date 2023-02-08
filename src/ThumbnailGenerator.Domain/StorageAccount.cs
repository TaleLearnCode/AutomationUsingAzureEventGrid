namespace TaleLearnCode.ThumbnailGenerator.Domain;

public class StorageAccount
{

	public StorageAccount()
	{
		ThumbnailGeneratorConfigOriginStorageAccounts = new HashSet<ThumbnailGeneratorConfig>();
		ThumbnailGeneratorConfigThumbnailStorageAccounts = new HashSet<ThumbnailGeneratorConfig>();
	}

	public int StorageAccountId { get; set; }
	public string ResourceName { get; set; }
	public string ContainerName { get; set; }
	public string FolderPath { get; set; }
	public bool? AllowSubfolders { get; set; }
	public bool CascadeDeletes { get; set; }

	public virtual ICollection<ThumbnailGeneratorConfig> ThumbnailGeneratorConfigOriginStorageAccounts { get; set; }
	public virtual ICollection<ThumbnailGeneratorConfig> ThumbnailGeneratorConfigThumbnailStorageAccounts { get; set; }

}