global using Fadi.Result;
global using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.User.Components;
using Modules.User.Features.Persistence;
using Shared.Features.Configuration;

namespace Modules.User.Features;
public static class Program
{
	public static IServiceCollection AddUserModuleServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
	{
		services.AddUserModuleComponentsServices(config);

		var _persistenceOptions = config.GetSection(PersistenceConfiguration.SectionName)
			.Get<PersistenceConfiguration>() ?? new();

		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
		});

		services.AddDbContext<UserContext>(options =>
		{
			options.UseSqlServer(_persistenceOptions.ConnectionString);
		});

		services.AddTransient<IUserContext, UserContext>();

		return services;
	}

	public static RazorComponentsEndpointConventionBuilder MapUserModulePages(this RazorComponentsEndpointConventionBuilder builder)
	{
		return builder.AddAdditionalAssemblies(typeof(Components.Program).Assembly);
	}
}
