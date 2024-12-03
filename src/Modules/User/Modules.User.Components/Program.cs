using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.User.Components;
public static class Program
{
	public static IServiceCollection AddUserModuleComponentsServices(this IServiceCollection services, IConfiguration config)
	{
		services.AddValidatorsFromAssemblyContaining<Integration.Program>();

		return services;
	}
}
