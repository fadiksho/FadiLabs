using Azure.Identity;


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

		services.AddRazorComponents(options =>
		{
			options.DetailedErrors = env.IsDevelopment();
		})
		 .AddInteractiveServerComponents()
		 .AddInteractiveWebAssemblyComponents();

		// ToDo: Testing progress
#pragma warning disable EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
		services.AddHybridCache();
#pragma warning restore EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

		return services;
	}
}
