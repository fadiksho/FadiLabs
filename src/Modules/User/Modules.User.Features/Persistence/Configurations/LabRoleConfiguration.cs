using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.User.Features.Entities;

namespace Modules.User.Features.Persistence.Configurations;
internal class LabRoleConfiguration : IEntityTypeConfiguration<LabRole>
{
	public void Configure(EntityTypeBuilder<LabRole> builder)
	{
		builder.Property(x => x.Name)
			.HasMaxLength(50);
	}
}
