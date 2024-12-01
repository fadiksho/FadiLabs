using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Blog.Components;

public static class Program
{
	public static IServiceCollection AddBlogModuleComponentsServices(this IServiceCollection services, IConfiguration config)
	{
		services.AddValidatorsFromAssemblyContaining<Integration.Program>();

		return services;
	}
}
