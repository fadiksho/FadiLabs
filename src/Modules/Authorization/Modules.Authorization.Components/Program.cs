using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Authorization.Integration.Authorization;

namespace Modules.Authorization.Components;
public static class Program
{
	public static IServiceCollection AddAuthorizationModuleComponentsServices(this IServiceCollection services, IConfiguration config)
	{
		services.AddValidatorsFromAssemblyContaining<Integration.Program>();

		services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
		services.AddSingleton<IAuthorizationPolicyProvider, LabAuthorizationPolicyProvider>();

		return services;
	}
}
