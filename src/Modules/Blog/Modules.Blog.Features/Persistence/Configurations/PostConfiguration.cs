using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Blog.Features.Entities;

namespace Modules.Blog.Features.Persistence.Configurations;

internal class PostConfiguration : IEntityTypeConfiguration<Post>
{
	public void Configure(EntityTypeBuilder<Post> builder)
	{
		builder
			.HasMany(x => x.Tags)
			.WithMany(x => x.Posts);

		builder
			.HasMany(x => x.Comments)
			.WithOne()
			.HasForeignKey(x => x.PostId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
