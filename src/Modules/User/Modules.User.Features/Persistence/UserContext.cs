using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Modules.User.Features.Entities;
using Shared.Features.Configuration;
using Shared.Features.Persistence;

namespace Modules.User.Features.Persistence;

public interface IUserContext : IModuleDbContext
{
	public DbSet<LabUser> LabUsers { get; }
	public DbSet<LabRole> LabRoles { get; }
}

public class UserContext
	(DbContextOptions<UserContext> options,
	IOptions<PersistenceConfiguration> persistenceOptions)
		: ModuleDbContext(options, persistenceOptions), IUserContext
{
	protected override string Schema => "User";

	public DbSet<LabUser> LabUsers { get; set; } = default!;
	public DbSet<LabRole> LabRoles { get; set; } = default!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
	}
}
