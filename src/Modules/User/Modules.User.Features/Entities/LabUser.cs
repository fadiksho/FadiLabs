﻿using Shared.Integration.Domain;

namespace Modules.User.Features.Entities;

public class LabUser : BaseEntity
{
	public required string Auth0UserId { get; set; }

	public string? DisplayName { get; set; }
	public string? Email { get; set; }
	public bool EmailVerified { get; set; }
	public string? ProfilePictureUrl { get; set; }

	public List<LabRole> LabRoles { get; set; } = [];
}
