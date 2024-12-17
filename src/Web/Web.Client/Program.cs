using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Modules.Auth0.Components;
using Modules.Blog.Components;
using Modules.User.Components;
using Shared.Components;
using Web.Client.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSharedComponentsServices(builder.Configuration)
	.AddAuth0ModuleComponentsServices(builder.Configuration)
	.AddUserModuleComponentsServices(builder.Configuration)
	.AddBlogModuleComponentsServices(builder.Configuration);

builder.Services.AddWasmServices(builder);

await builder.Build().RunAsync();

namespace Web.Client
{
	public class Program { }
}