using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Features.Configuration;
using Shared.Integration.Domain;

namespace Shared.Features.Persistence;

public abstract class ModuleDbContext : DbContext, IModuleDbContext
{
	private readonly PersistenceConfiguration _persistenceOptions;

	protected abstract string Schema { get; }

	protected ModuleDbContext(
		DbContextOptions options,
		IOptions<PersistenceConfiguration> persistenceOptions) : base(options)
	{
		_persistenceOptions = persistenceOptions.Value;
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(Schema);
		modelBuilder.Ignore<EntityEvent>();
		base.OnModelCreating(modelBuilder);
	}
}
