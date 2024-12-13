using Azure.Identity;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Components;
using Shared.Components.Services;
using Shared.Features.Behaviours;
using Shared.Features.Configuration;
using Shared.Features.Services;
using Shared.Features.Services.Implementaions;
using Shared.Integration.Services;
using Web.Server.Services;


namespace Web.Server.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddServerServices(this IServiceCollection services, IConfigurationManager config, IWebHostEnvironment env)
	{
		if (env.IsProduction())
		{
			config.AddAzureKeyVault(
					new Uri($"https://{config["KeyVaultName"]}.vault.azure.net/"),
					new DefaultAzureCredential());
		}

		services.AddRazorComponents()
		 .AddInteractiveServerComponents()
		 .AddInteractiveWebAssemblyComponents();

		services.AddMediator(config);
		services.AddConfigurationSettings(config);

		services.AddHttpContextAccessor();

		services.AddSharedComponentsServices(config);

		services.AddScoped<AuthenticationStateProvider, ServerASP>();
		services.AddScoped<IAuthService, ServerAuthService>();

		services.AddSingleton<LogIndexer>();

		services.AddScoped<IMessageSender, ServerMessageSender>();
		services.AddScoped<ICurrentUser, ServerCurrentUser>();
		services.AddScoped<ITokenService, ServerTokenService>();
#pragma warning disable EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
		services.AddHybridCache();
#pragma warning restore EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

		return services;
	}

	private static IServiceCollection AddMediator(this IServiceCollection services, IConfiguration config)
	{
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
		});

		return services;
	}

	private static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration config)
	{
		services.Configure<PersistenceConfiguration>(config.GetSection(PersistenceConfiguration.SectionName));
		services.Configure<DevTunnelConfiguration>(config.GetSection(DevTunnelConfiguration.SectionName));
		return services;
	}
}
