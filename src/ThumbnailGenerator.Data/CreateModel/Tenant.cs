namespace TaleLearnCode.ThumbnailGenerator.Data;

internal static partial class CreateModel
{

	internal static void Tenant(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Tenant>(entity =>
		{
			entity.ToTable("Tenant", "dbo");
			entity.Property(e => e.TenantKey).IsRequired().HasMaxLength(36).IsUnicode(false);
			entity.Property(e => e.TenantName).IsRequired().HasMaxLength(100);
			entity.Property(e => e.DatabaseServerName).IsRequired().HasMaxLength(100);
			entity.Property(e => e.DatabaseName).IsRequired().HasMaxLength(100);
		});
	}

}