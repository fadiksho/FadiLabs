using Modules.Auth0.Features;
using Modules.Blog.Features;
using Modules.Home.Features;
using Modules.User.Features;
using Shared.Features;
using Web.Server.Endpoints;
using Web.Server.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(options => options.AddServerHeader = false);

builder.Services
	.AddServerServices(builder.Configuration, builder.Environment)
	.AddSharedModuleServices(builder.Configuration, builder.Environment)
	.AddHomeModuleServices(builder.Configuration, builder.Environment)
	.AddAuth0ModuleServices(builder.Configuration, builder.Environment)
	.AddUserModuleServices(builder.Configuration, builder.Environment)
	.AddBlogModuleServices(builder.Configuration, builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
	//app.UseMiddleware<DevMigrationsMiddleware>();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapBlazorApp()
	 .MapHomeModulePages()
	 .MapAuth0ModulePages()
	 .MapUserModulePages()
	 .MapBlogModulePages();

app
	.MapMediatorEndPoints();
app
	.MapAuth0ModleEndPoints();

app.Run();

namespace Web.Server
{
	public class Program { }
}