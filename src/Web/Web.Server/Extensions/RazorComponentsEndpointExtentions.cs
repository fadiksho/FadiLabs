namespace Web.Server.Extensions;

public static class RazorComponentsEndpointExtentions
{
	public static RazorComponentsEndpointConventionBuilder MapBlazorApp(this IEndpointRouteBuilder app)
	{
		var componentsEndPointBuilder = app.MapRazorComponents<Components.App>()
			.AddInteractiveServerRenderMode()
			.AddInteractiveWebAssemblyRenderMode()
			.AddAdditionalAssemblies(typeof(Client.Program).Assembly);

		return componentsEndPointBuilder;
	}
}
