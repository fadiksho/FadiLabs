using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Modules.Shared.Integration.Domain;
using Shared.Features.Configuration;

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
		modelBuilder.Ignore<BaseEntity>();

		base.OnModelCreating(modelBuilder);
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		//configurationBuilder.Properties<string>()
		//	.HaveMaxLength(256);

		base.ConfigureConventions(configurationBuilder);
	}
}
