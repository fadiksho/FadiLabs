using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Components;
using Shared.Components.Services;
using Shared.Features.Server.Behaviours;
using Shared.Features.Server.Configuration;
using Shared.Features.Server.Endpoints;
using Shared.Features.Server.Persistence.Interceptors;
using Shared.Features.Server.Services;
using Shared.Features.Server.Services.Implementaions;

namespace Shared.Features.Server;

public static class Program
{
	public static IServiceCollection AddSharedModuleServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
	{
		services.AddSharedComponentsServices(config);

		services.AddScoped<LazyAssemblyLoader>();

		services.AddConfigurationSettings(config);

		services.AddCircuitServicesAccessor();
		services.AddScoped<ICurrentUser, ServerCurrentUser>();

		services.AddScoped<AuthenticationStateProvider, ServerASP>();
		services.AddScoped<IAuthService, ServerAuthService>();
		services.AddScoped<IMessageSender, ServerMessageSender>();

		services.AddHttpClient();
		services.AddHttpContextAccessor();

		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
		});

		services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
		services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

		return services;
	}

	public static RazorComponentsEndpointConventionBuilder MapSharedModulePages(this RazorComponentsEndpointConventionBuilder builder)
	{
		return builder.AddAdditionalAssemblies(typeof(Components.Program).Assembly);
	}

	public static void MapSharedEndPoints(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapMediatorEndPoints();
	}

	private static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration config)
	{
		services.Configure<PersistenceConfiguration>(config.GetSection(PersistenceConfiguration.SectionName));
		services.Configure<DevTunnelConfiguration>(config.GetSection(DevTunnelConfiguration.SectionName));
		return services;
	}

	private static IServiceCollection AddCircuitServicesAccessor(
				this IServiceCollection services)
	{
		services.AddScoped<CircuitServicesAccessor>();
		services.AddScoped<CircuitHandler, ServicesAccessorCircuitHandler>();

		return services;
	}
}
