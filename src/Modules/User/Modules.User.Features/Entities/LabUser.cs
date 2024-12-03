using Shared.Integration.Domain;

namespace Modules.User.Features.Entities;

public class LabUser : BaseEntity
{
	public required string Auth0UserId { get; set; }
	public required string Email { get; set; }
	public string? ProfilePictureUrl { get; set; }

	public List<LinkedAccount> LinkedAccounts { get; set; } = [];
}
