namespace Shared.Features.Configuration;

public class PersistenceConfiguration
{
	public static readonly string SectionName = "Persistence";
	public string ConnectionString { get; set; } = default!;
}
