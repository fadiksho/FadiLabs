using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Modules.Home.Components;
using Shared.Features.Wasm;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
	.AddSharedServices(builder.Configuration, builder.HostEnvironment)
	.AddHomeModuleComponentsServices(builder.Configuration);

await builder.Build().RunAsync();

namespace Web.Client
{
	public class Program { }
}