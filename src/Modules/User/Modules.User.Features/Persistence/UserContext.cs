using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Modules.User.Features.Entities;
using Shared.Features.Configuration;
using Shared.Features.Persistence;

namespace Modules.User.Features.Persistence;

public interface IUserContext : IModuleDbContext
{
	public DbSet<LabUser> LabUsers { get; }
}

public class UserContext : ModuleDbContext, IUserContext
{
	protected override string Schema => "User";

	public DbSet<LabUser> LabUsers { get; set; } = default!;

	public UserContext(
			 DbContextOptions<UserContext> options,
					IOptions<PersistenceConfiguration> persistenceOptions)
		: base(options, persistenceOptions)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<LabUser>(labUser =>
		{
			labUser.HasMany(e => e.LinkedAccounts)
								.WithOne()
								.HasForeignKey(ul => ul.LabUserId)
								.IsRequired();
		});

		modelBuilder.Entity<LinkedAccount>(linkedAccount =>
		{
			linkedAccount.HasKey(l => new { l.Provider, l.ProviderKey });

			linkedAccount.Property(l => l.Provider).HasMaxLength(128);
			linkedAccount.Property(l => l.ProviderKey).HasMaxLength(128);
		});
	}
}
