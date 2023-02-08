namespace TaleLearnCode.ThumbnailGenerator.Data;

internal static partial class CreateModel
{

	internal static void StorageAccount(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<StorageAccount>(entity =>
		{
			entity.ToTable("StorageAccount", "dbo");
			entity.Property(e => e.StorageAccountId).ValueGeneratedNever();
			entity.Property(e => e.ResourceName).IsRequired().HasMaxLength(24).IsUnicode(false);
			entity.Property(e => e.ContainerName).IsRequired().HasMaxLength(63).IsUnicode(false);
			entity.Property(e => e.FolderPath).HasMaxLength(1024).IsUnicode(false);
		});
	}

}