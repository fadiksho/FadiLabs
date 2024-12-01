using Maui.Startup.Extensions;
using Microsoft.Extensions.Logging;
using Modules.Blog.Components;
using Shared.Components;
using Shared.Components.Services;

namespace Maui.Startup;
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
		InteractiveRenderSettings.ConfigureBlazorHybridRenderModes();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSharedComponentsServices(builder.Configuration)
			.AddBlogModuleComponentsServices(builder.Configuration);

		builder.Services.AddMauiConfiguration(builder.Configuration);
		builder.Services.AddMauiServices(builder.Configuration);

		return builder.Build();
	}
}
