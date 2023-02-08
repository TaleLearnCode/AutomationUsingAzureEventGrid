namespace TaleLearnCode.ThumbnailGenerator.Data;

internal static partial class CreateModel
{
	internal static void ThumbnailGeneratorConfig(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ThumbnailGeneratorConfig>(entity =>
		{
			entity.ToTable("ThumbnailGeneratorConfig", "dbo");
			entity.Property(e => e.ThumbnailGeneratorConfigId).ValueGeneratedNever();
			entity.Property(e => e.ThumbnailGeneratorConfigName).IsRequired().HasMaxLength(100).IsUnicode(false);
			entity.Property(e => e.ThumbnailPrefix).HasMaxLength(10).IsUnicode(false);
			entity.Property(e => e.ThumbnailSuffix).HasMaxLength(10).IsUnicode(false);
			entity.HasOne(d => d.OriginStorageAccount)
				.WithMany(p => p.ThumbnailGeneratorConfigOriginStorageAccounts)
				.HasForeignKey(d => d.OriginStorageAccountId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fkThumbnailGeneratorConfig_StorageAccount_OriginStorageAccountId");
			entity.HasOne(d => d.ThumbnailStorageAccount)
				.WithMany(p => p.ThumbnailGeneratorConfigThumbnailStorageAccounts)
				.HasForeignKey(d => d.ThumbnailStorageAccountId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fkThumbnailGeneratorConfig_StorageAccount_ThumbnailStorageAccountId");
		});
	}
}