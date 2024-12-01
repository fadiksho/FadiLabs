using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Auth0.Components;

public static class Program
{
	public static IServiceCollection AddAuth0ModuleComponentsServices(this IServiceCollection services, IConfiguration config)
	{
		services.AddValidatorsFromAssemblyContaining<Integration.Program>();

		return services;
	}
}