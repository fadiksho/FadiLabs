using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared.Components.Services;
using Web.Client.Services;

namespace Web.Client.Extensions;
internal static class ServiceCollectionExtensions
{
	public static IServiceCollection AddWasmServices(this IServiceCollection services, WebAssemblyHostBuilder builder)
	{
		services.AddHttpClient<IMessageSender, WasmMessageSender>((sp, client) =>
		{
			client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
		});
		//.RemoveAllLoggers();

		services.AddSingleton<AuthenticationStateProvider, WasmASP>();
		services.AddScoped<IAuthService, WasmAuthService>();

		return services;
	}
}
