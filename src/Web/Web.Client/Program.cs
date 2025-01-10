using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Modules.Home.Components;
using Shared.Components;
using Web.Client.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSharedComponentsServices(builder.Configuration)
	.AddHomeModuleComponentsServices(builder.Configuration);
//.AddAuth0ModuleComponentsServices(builder.Configuration)
//.AddUserModuleComponentsServices(builder.Configuration)
//.AddBlogModuleComponentsServices(builder.Configuration);

builder.Services.AddWasmServices(builder);

await builder.Build().RunAsync();

namespace Web.Client
{
	public class Program { }
}