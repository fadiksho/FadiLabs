using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Static;
public static class Program
{
	public static IServiceCollection AddStaticComponentsServices(this IServiceCollection services, IConfiguration config)
	{
		return services;
	}

	public static RazorComponentsEndpointConventionBuilder MapStaticPages(this RazorComponentsEndpointConventionBuilder builder)
	{
		return builder.AddAdditionalAssemblies(typeof(Program).Assembly);
	}
}
