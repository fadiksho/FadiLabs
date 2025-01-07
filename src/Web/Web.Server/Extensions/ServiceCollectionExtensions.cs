using Azure.Identity;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Components.Services;
using Shared.Features.Behaviours;
using Shared.Features.Configuration;
using Shared.Features.Persistence.Interceptors;
using Shared.Features.Services;
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

		services.AddHttpClient();
		services.AddHttpContextAccessor();
		services.AddRazorComponents(options =>
		{
			options.DetailedErrors = env.IsDevelopment();
		})
		 .AddInteractiveServerComponents()
		 .AddInteractiveWebAssemblyComponents();

		services.AddMediator(config);
		services.AddConfigurationSettings(config);

		services.AddCircuitServicesAccessor();
		services.AddScoped<ICurrentUser, ServerCurrentUser>();

		services.AddScoped<AuthenticationStateProvider, ServerASP>();
		services.AddScoped<IAuthService, ServerAuthService>();
		services.AddScoped<IMessageSender, ServerMessageSender>();

		// ToDo: Testing progress
#pragma warning disable EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
		services.AddHybridCache();
#pragma warning restore EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

		services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
		services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

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
