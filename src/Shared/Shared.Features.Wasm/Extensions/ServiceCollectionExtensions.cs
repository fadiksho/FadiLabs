using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Components.Services;
using Shared.Features.Wasm.Services;

namespace Shared.Features.Wasm.Extensions;

internal static class ServiceCollectionExtensions
{
	public static IServiceCollection AddWasmServices(this IServiceCollection services, IConfiguration config, IWebAssemblyHostEnvironment env)
	{
		services.AddHttpClient<IMessageSender, WasmMessageSender>((sp, client) =>
		{
			client.BaseAddress = new Uri(env.BaseAddress);
		});
		//.RemoveAllLoggers();

		services.AddSingleton<AuthenticationStateProvider, WasmASP>();
		services.AddScoped<IAuthService, WasmAuthService>();

		return services;
	}
}
