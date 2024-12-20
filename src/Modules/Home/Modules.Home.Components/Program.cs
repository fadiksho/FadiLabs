using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Home.Components;

public static class Program
{
	public static IServiceCollection AddHomeModuleComponentsServices(this IServiceCollection services, IConfiguration config)
	{
		services.AddValidatorsFromAssemblyContaining<Integration.Program>();

		return services;
	}
}