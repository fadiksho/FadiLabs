using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Blog.Components;
using Modules.Blog.Features.Persistence;
using Shared.Features.Configuration;
using Shared.Features.Persistence;
namespace Modules.Blog.Features;

public static class Program
{
	public static IServiceCollection AddBlogModuleServices(this IServiceCollection services, IConfiguration config)
	{
		services.AddBlogModuleComponentsServices(config);

		var _persistenceOptions = config.GetSection(PersistenceConfiguration.SectionName)
			.Get<PersistenceConfiguration>() ?? new();

		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
		});

		services.AddDbContext<BlogContext>(options =>
		{
			options.UseSqlServer(_persistenceOptions.ConnectionString);
			options.ConfigureWarnings(warningsConfig =>
			{
				warningsConfig.Ignore(RelationalEventId.PendingModelChangesWarning);
			});
		});

		services.AddTransient<IBlogContext, BlogContext>();
		services.AddScoped<IContextSeed, BlogContextSeed>();

		return services;
	}

	public static RazorComponentsEndpointConventionBuilder MapBlogModulePages(this RazorComponentsEndpointConventionBuilder builder)
	{
		return builder.AddAdditionalAssemblies(typeof(Components.Program).Assembly);
	}
}
