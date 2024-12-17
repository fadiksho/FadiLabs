using Auth0.OidcClient;
using Maui.Startup.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Modules.Auth0.Integration.Configuration;
using Shared.Components.Services;
using System.Reflection;

namespace Maui.Startup.Extensions;

internal static class ServiceCollectionExtensions
{
	public static IServiceCollection AddMauiServices(this IServiceCollection services, IConfiguration config)
	{
		services.AddTransient<TokenHandler>();
		services.AddHttpClient(
			"MauiHttpClient", client => client.BaseAddress = new Uri("https://localhost:7056/"))
			.AddHttpMessageHandler<TokenHandler>();

		services.AddTransient(
			sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("MauiHttpClient"));

		var _auth0Options = config.GetSection(Auth0Configuration.SectionName)
			.Get<Auth0Configuration>() ?? new();

		services.AddSingleton(new Auth0Client(new()
		{
			Domain = _auth0Options.Domain,
			ClientId = _auth0Options.ClientId,
			Scope = "openid profile offline_access",
			RedirectUri = "fadilabs://callback/",
			PostLogoutRedirectUri = "fadilabs://callback/"
		}));

		services.AddScoped<IMessageSender, MauiMessageSender>();
		services.AddScoped<AuthenticationStateProvider, MauiASP>();
		services.AddScoped<IAuthService, MauiAuthService>();
		services.AddSingleton<MauiUserService>();

#if ANDROID

#elif iOS
        
#elif Mac
        
#elif Windows
        
#endif

		return services;
	}

	public static IServiceCollection AddMauiConfiguration(this IServiceCollection services, ConfigurationManager config)
	{
		using var appsettingsStream = Assembly
						.GetExecutingAssembly()
						.GetManifestResourceStream("Maui.Startup.wwwroot.appsettings.json");

		if (appsettingsStream != null)
		{
			var configurationBuilder = new ConfigurationBuilder()
					.AddJsonStream(appsettingsStream)
					.Build();

			config.AddConfiguration(configurationBuilder);
		}

		services
			.Configure<Auth0Configuration>(config.GetSection(Auth0Configuration.SectionName));

		return services;
	}
}