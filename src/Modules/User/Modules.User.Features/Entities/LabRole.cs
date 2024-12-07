using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Integration.Authorization;
using Shared.Integration.Domain;

namespace Modules.User.Features.Entities;

public class LabRole : IEntity<Guid>
{
	public Guid Id { get; set; }

	public required string Name { get; set; }
	public required string Description { get; set; }

	public Permissions Permissions { get; set; } = Permissions.None;

	public List<LabUser> LabUsers { get; set; } = [];
}

internal class LabRoleConfiguration : IEntityTypeConfiguration<LabRole>
{
	public void Configure(EntityTypeBuilder<LabRole> builder)
	{
		builder.HasIndex(x => x.Name)
			.IsUnique();

		builder.Property(x => x.Name)
			.HasMaxLength(50);
	}
}