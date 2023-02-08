#nullable disable

namespace TaleLearnCode.ThumbnailGenerator.Data;

public class ThumbnailGeneratorContext : DbContext
{

	private readonly string _connectionString;

	public ThumbnailGeneratorContext(string connectionString) => _connectionString = connectionString;
	public ThumbnailGeneratorContext(DbContextOptions<ThumbnailGeneratorContext> options) : base(options) { }


	public DbSet<StorageAccount> StorageAccounts { get; set; }
	public DbSet<Tenant> Tenants { get; set; }
	public DbSet<ThumbnailGeneratorConfig> ThumbnailGeneratorConfigs { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder
			.UseSqlServer(_connectionString);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		CreateModel.StorageAccount(modelBuilder);
		CreateModel.Tenant(modelBuilder);
		CreateModel.ThumbnailGeneratorConfig(modelBuilder);
	}

}