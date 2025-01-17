﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Integration.Domain;
using Modules.Shared.Integration.Domain.Contracts;

namespace Modules.User.Features.Entities;

public class LabUser : AuditableEntity, IEntity<Guid>
{
	public Guid Id { get; set; }
	public required string Auth0UserId { get; set; }
	public required string Email { get; set; }

	public string? DisplayName { get; set; }
	public bool EmailVerified { get; set; }
	public string? ProfilePictureUrl { get; set; }

	public List<LabRole> LabRoles { get; set; } = [];
}

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