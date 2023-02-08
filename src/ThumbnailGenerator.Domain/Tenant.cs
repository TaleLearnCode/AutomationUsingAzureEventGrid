namespace TaleLearnCode.ThumbnailGenerator.Domain;

public class Tenant
{
	public int TenantId { get; set; }
	public string TenantKey { get; set; }
	public string TenantName { get; set; }
	public string DatabaseServerName { get; set; }
	public string DatabaseName { get; set; }
}