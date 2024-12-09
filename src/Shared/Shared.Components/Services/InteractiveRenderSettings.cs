using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Shared.Components.Services;
public static class InteractiveRenderSettings
{
	public static IComponentRenderMode? InteractiveServer { get; set; } =
			new InteractiveServerRenderMode(prerender: true);
	public static IComponentRenderMode? InteractiveAuto { get; set; } =
			new InteractiveAutoRenderMode(prerender: true);
	public static IComponentRenderMode? InteractiveWebAssembly { get; set; } =
			new InteractiveWebAssemblyRenderMode(prerender: true);

	public static void ConfigureBlazorHybridRenderModes()
	{
		InteractiveServer = null;
		InteractiveAuto = null;
		InteractiveWebAssembly = null;
	}
}
