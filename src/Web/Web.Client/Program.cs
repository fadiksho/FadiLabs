using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Modules.Blog.Components;
using Shared.Components;
using Web.Client.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSharedComponentsServices(builder.Configuration)
	.AddBlogModuleComponentsServices(builder.Configuration);
builder.Services.AddWasmServices(builder);

await builder.Build().RunAsync();

namespace Web.Client
{
	internal class Program { }
}
