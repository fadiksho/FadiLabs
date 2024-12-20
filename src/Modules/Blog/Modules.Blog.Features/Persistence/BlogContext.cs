using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Modules.Blog.Features.Entities;
using Shared.Features.Configuration;
using Shared.Features.Persistence;

namespace Modules.Blog.Features.Persistence;

public interface IBlogContext : IModuleDbContext
{
	DbSet<Post> Posts { get; }
	DbSet<Tag> Tags { get; }
	DbSet<Comment> Comments { get; }
}

public class BlogContext
	(DbContextOptions<BlogContext> options,
	IOptions<PersistenceConfiguration> persistenceOptions)
		: ModuleDbContext(options, persistenceOptions), IBlogContext
{
	public DbSet<Post> Posts { get; set; } = default!;
	public DbSet<Tag> Tags { get; set; } = default!;
	public DbSet<Comment> Comments { get; set; } = default!;

	protected override string Schema => "Blog";

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
	}
}
