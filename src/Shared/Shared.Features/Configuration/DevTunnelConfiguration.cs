namespace Shared.Features.Server.Configuration;

public class DevTunnelConfiguration
{
	public static readonly string SectionName = "DevTunnel";
	public bool IsEnabled { get; set; }
	public string Url { get; set; } = default!;
}
