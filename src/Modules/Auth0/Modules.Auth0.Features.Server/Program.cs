using Microsoft.AspNetCore.Builder;

namespace Modules.Auth0.Features.Server;

public static class Program
{
	public static RazorComponentsEndpointConventionBuilder MapOAuthModuleServerPages(this RazorComponentsEndpointConventionBuilder builder)
	{
		return builder.AddAdditionalAssemblies(
			typeof(Program).Assembly);
	}
}
