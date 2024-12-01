namespace Modules.Auth0.Features.Configuration;
public class Auth0Configuration
{
	public static readonly string SectionName = "Modules:Auth0";
	public string Domain { get; set; } = default!;
	public string ClientId { get; set; } = default!;
	public string ClientSecret { get; set; } = default!;
	public string Audience { get; set; } = default!;
}
