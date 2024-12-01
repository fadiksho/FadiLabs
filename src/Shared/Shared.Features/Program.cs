using Microsoft.AspNetCore.Builder;

namespace Shared.Features;
public static class Program
{
	public static RazorComponentsEndpointConventionBuilder MapSharedModulePages(this RazorComponentsEndpointConventionBuilder builder)
	{
		return builder.AddAdditionalAssemblies(typeof(Components.Program).Assembly);
	}
}
