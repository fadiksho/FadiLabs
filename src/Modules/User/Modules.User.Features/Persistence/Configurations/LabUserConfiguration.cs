using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.User.Features.Entities;

namespace Modules.User.Features.Persistence.Configurations;

internal class LabUserConfiguration : IEntityTypeConfiguration<LabUser>
{
	public void Configure(EntityTypeBuilder<LabUser> builder)
	{
		builder.HasIndex(x => x.Auth0UserId)
			.IsUnique();

		builder.Property(x => x.Auth0UserId)
			.HasMaxLength(50);

		builder.Property(x => x.Email)
			.IsRequired()
			.HasMaxLength(50);
	}
}
