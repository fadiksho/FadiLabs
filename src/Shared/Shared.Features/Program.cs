using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Components;
using Shared.Features.Endpoints;

namespace Shared.Features;
public static class Program
{
	public static IServiceCollection AddSharedModuleServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
	{
		services.AddSharedComponentsServices(config);

		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
		});

		return services;
	}

	public static void MapSharedEndPoints(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapMediatorEndPoints();
	}
}
