namespace Web.Server.Extensions;

public static class RazorComponentsEndpointExtentions
{
	public static RazorComponentsEndpointConventionBuilder MapBlazorApp(this IEndpointRouteBuilder app)
	{
		var componentsEndPointBuilder = app.MapRazorComponents<Components.App>()
			.AddInteractiveServerRenderMode()
			.AddInteractiveWebAssemblyRenderMode();

		return componentsEndPointBuilder;
	}
}
