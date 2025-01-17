﻿using Maui.Startup.Extensions;
using Microsoft.Extensions.Logging;
using Modules.Auth0.Components;
using Modules.Blog.Components;
using Modules.Home.Components;
using Modules.User.Components;
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
			.AddHomeModuleComponentsServices(builder.Configuration)
			.AddAuth0ModuleComponentsServices(builder.Configuration)
			.AddUserModuleComponentsServices(builder.Configuration)
			.AddBlogModuleComponentsServices(builder.Configuration);

		builder.Services.AddMauiConfiguration(builder.Configuration);
		builder.Services.AddMauiServices(builder.Configuration);

		return builder.Build();
	}
}
