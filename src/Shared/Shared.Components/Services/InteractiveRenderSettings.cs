using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Shared.Components.Services;
public static class InteractiveRenderSettings
{
	public static IComponentRenderMode? InteractiveServer { get; set; } =
			new InteractiveServerRenderMode(prerender: false);
	public static IComponentRenderMode? InteractiveAuto { get; set; } =
			new InteractiveAutoRenderMode(prerender: false);
	public static IComponentRenderMode? InteractiveWebAssembly { get; set; } =
			new InteractiveWebAssemblyRenderMode(prerender: false);

	public static void ConfigureBlazorHybridRenderModes()
	{
		InteractiveServer = null;
		InteractiveAuto = null;
		InteractiveWebAssembly = null;
	}
}
