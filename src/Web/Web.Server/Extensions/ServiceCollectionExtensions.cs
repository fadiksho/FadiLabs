using Azure.Identity;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Components;
using Shared.Components.Services;
using Shared.Features.Behaviours;
using Shared.Features.Configuration;
using Shared.Features.Services;
using Shared.Integration.Services;
using Web.Server.Services;
using Web.Server.Services.Temp;


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

		services.AddSharedComponentsServices(config);

		services.AddHttpContextAccessor();
		services.AddRazorComponents()
		 .AddInteractiveServerComponents()
		 .AddInteractiveWebAssemblyComponents();

		services.AddMediator(config);
		services.AddConfigurationSettings(config);


		services.AddCircuitServicesAccessor();
		services.AddScoped<ActiveCircuitState>();
		//services.AddScoped<CircuitHandler, ActiveCircuitStateHandler>();
		//services.AddScoped<CircuitHandler, CurrentUserCircuitHandler>();
		services.TryAddEnumerable([
			ServiceDescriptor.Scoped<CircuitHandler, ActiveCircuitStateHandler>(),
			ServiceDescriptor.Scoped<CircuitHandler, CurrentUserCircuitHandler>()
		]);
		services.AddScoped<ICurrentUser, ServerCurrentUser>();

		//services.AddScoped<AuthenticationStateProvider, ServerASP>();
		services.AddScoped<AuthenticationStateProvider, ServerASP2>();
		services.AddScoped<IAuthService, ServerAuthService>();
		services.AddScoped<IMessageSender, ServerMessageSender>();

		// ToDo: Testing progress
#pragma warning disable EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
		services.AddHybridCache();
#pragma warning restore EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

		// ToDo: Delete
		services.AddScoped<ITokenService, ServerTokenService>();

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

	private static IServiceCollection AddCircuitServicesAccessor(
				this IServiceCollection services)
	{

		services.AddScoped<CircuitServicesAccessor>();
		services.AddScoped<CircuitHandler, ServicesAccessorCircuitHandler>();

		return services;
	}
}
