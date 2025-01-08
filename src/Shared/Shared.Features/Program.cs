using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Components;
using Shared.Features.Behaviours;
using Shared.Features.Endpoints;

namespace Shared.Features;
public static class Program
{
	public static IServiceCollection AddSharedModuleServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
	{
		services.AddSharedComponentsServices(config);

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
}
