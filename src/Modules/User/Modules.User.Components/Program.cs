using Fadi.Result.Serialization;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Modules.User.Integration;

namespace Modules.User.Components;
public static class Program
{
	public static IServiceCollection AddUserModuleComponentsServices(this IServiceCollection services, IConfiguration config)
	{
		services.AddValidatorsFromAssemblyContaining<Integration.Program>();
		services.TryAddSingleton<IResultErrorPolymorphicResolver, ResultErrorPolymorphicResolver>();

		return services;
	}
}
