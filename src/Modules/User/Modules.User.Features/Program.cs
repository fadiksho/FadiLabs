﻿global using Fadi.Result;
global using Fadi.Result.Errors;
global using MediatR;
global using Modules.User.Integration.User.Commands;
global using Modules.User.Integration.User.Events;
global using Modules.User.Integration.User.Queries;
global using Modules.User.Integration.User.ResultErrors;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

		services.AddDbContext<UserContext>((sp, options) =>
		{
			options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
			options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
			options.UseSqlServer(_persistenceOptions.ConnectionString, providerOptions =>
			{
				providerOptions.EnableRetryOnFailure(maxRetryCount: 3);
			});
		});

		services.AddTransient<IUserContext, UserContext>();

		return services;
	}

	public static RazorComponentsEndpointConventionBuilder MapUserModulePages(this RazorComponentsEndpointConventionBuilder builder)
	{
		return builder.AddAdditionalAssemblies(typeof(Components.Program).Assembly);
	}
}
