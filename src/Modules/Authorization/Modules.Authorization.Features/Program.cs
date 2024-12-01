using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Authorization.Features;
public static class Program
{
	public static IServiceCollection AddAuthorizationModuleServices(this IServiceCollection services, IConfiguration config)
	{
		return services;
	}
}
