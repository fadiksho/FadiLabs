﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Integration.Authorization;
using Modules.Shared.Integration.Domain;
using Modules.Shared.Integration.Domain.Contracts;

namespace Modules.User.Features.Entities;

public class LabRole : AuditableEntity, IEntity<Guid>
{
	public Guid Id { get; set; }

	public required string Name { get; set; }
	public required string Description { get; set; }

	public bool AutoAssign { get; set; }
	public LabsPermissions LabsPermissions { get; set; } = LabsPermissions.None;

	public List<LabUser> LabUsers { get; set; } = [];
}

internal class LabRoleConfiguration : IEntityTypeConfiguration<LabRole>
{
	public void Configure(EntityTypeBuilder<LabRole> builder)
	{
		//builder.Property(b => b.Id)
		//	.HasDefaultValueSql("NEWSEQUENTIALID()");

		builder.HasIndex(x => x.Name)
			.IsUnique();

		builder.Property(x => x.Name)
			.HasMaxLength(50);

		builder.HasData(
			new LabRole
			{
				Id = Guid.NewGuid(),
				Name = "admin",
				Description = "default admin role.",
				LabsPermissions = LabsPermissions.All,
				AutoAssign = false,
				Created = DateTimeOffset.UtcNow,
				LastModified = DateTimeOffset.UtcNow
			},
			new LabRole
			{
				Id = Guid.NewGuid(),
				Name = "lab user",
				Description = "default lab user role.",
				LabsPermissions = LabsPermissions.None,
				AutoAssign = true,
				Created = DateTimeOffset.UtcNow,
				LastModified = DateTimeOffset.UtcNow
			}
		);
	}
}