using Fadi.Result.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modules.Authorization.Integration.Authorization;
using Shared.Components.Services;
using Shared.Components.Services.Implementations;
using Shared.Integration.Services;
using Shared.Integration.Services.Implementaions;
using System.Reflection;

namespace Shared.Components;

public static class Program
{
	public static IServiceCollection AddSharedComponentsServices(this IServiceCollection services, IConfiguration configuration, params Assembly[] moduleAssemblies)
	{
		services.AddAuthorizationCore();
		services.AddCascadingAuthenticationState();
		services.AddScoped<IUIBus, DefaultUIBus>();
		services.AddScoped(typeof(IServiceExecutor<>), typeof(ServiceExecutor<>));
		services.AddScoped<IPrerenderState, PrerenderStateWithJsonOptions>();

		services.TryAddSingleton<IResultErrorPolymorphicResolver, DefaultResultErrorPolymorphicResolver>();
		services.TryAddSingleton<IEnvelopMessageHandler, DefaultEnvelopMessageHandler>();
		services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
		services.AddSingleton<IAuthorizationPolicyProvider, LabAuthorizationPolicyProvider>();

		return services;
	}
}
