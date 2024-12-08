using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Features.Configuration;
using Shared.Integration.Domain;

namespace Shared.Features.Persistence;

public abstract class ModuleDbContext(
	DbContextOptions options,
	IOptions<PersistenceConfiguration> persistenceOptions) : DbContext(options), IModuleDbContext
{
	private readonly PersistenceConfiguration _persistenceOptions = persistenceOptions.Value;

	protected abstract string Schema { get; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(Schema);
		modelBuilder.Ignore<EntityEvent>();

		base.OnModelCreating(modelBuilder);
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		//configurationBuilder.Properties<string>()
		//	.HaveMaxLength(256);

		base.ConfigureConventions(configurationBuilder);
	}
}
