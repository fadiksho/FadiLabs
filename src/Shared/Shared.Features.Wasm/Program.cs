using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Components;
using Shared.Features.Wasm.Extensions;

namespace Shared.Features.Wasm;

public static class Program
{
	public static IServiceCollection AddSharedServices(this IServiceCollection services, IConfiguration config, IWebAssemblyHostEnvironment env)
	{
		services
			.AddSharedComponentsServices(config)
			.AddWasmServices(config, env);

		return services;
	}
}
