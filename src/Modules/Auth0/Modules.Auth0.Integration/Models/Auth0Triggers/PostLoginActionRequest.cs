namespace Modules.Auth0.Integration.Models.Auth0Triggers;

public record PostLoginActionRequest
{
	public required string Auth0UserId { get; set; }
	public string? DisplayName { get; set; }
	public string? Email { get; set; }
	public bool EmailVerified { get; set; }
	public string? ProfilePictureUrl { get; set; }

	public bool IsLabUser { get; set; }
}
