global using Fadi.Result;
global using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Blog.Components;
using Modules.Blog.Features.Persistence;
using Shared.Features.Configuration;

namespace Modules.Blog.Features;

public static class Program
{
	public static IServiceCollection AddBlogModuleServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
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
			options.UseSqlServer(_persistenceOptions.ConnectionString)
				.UseSeeding(BlogContextSeed.Seed);
		});

		services.AddTransient<IBlogContext, BlogContext>();

		return services;
	}

	public static RazorComponentsEndpointConventionBuilder MapBlogModulePages(this RazorComponentsEndpointConventionBuilder builder)
	{
		return builder.AddAdditionalAssemblies(typeof(Components.Program).Assembly);
	}
}
