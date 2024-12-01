using System.Security.Claims;

namespace Shared.Integration.Models;

public sealed class UserInfo
{
	public required string UserId { get; init; }
	public required string Name { get; init; }
	public required string? Email { get; init; }


	public const string UserIdClaimType = ClaimTypes.NameIdentifier;
	public const string NameClaimType = "name";
	public const string EmailClaimType = ClaimTypes.Email;
	public static UserInfo BuildFromClaimPrincipal(ClaimsPrincipal principal)
	{
		return new UserInfo
		{
			UserId = GetRequiredClaim(principal, UserIdClaimType),
			Name = GetRequiredClaim(principal, NameClaimType),
			Email = principal.FindFirst(EmailClaimType)?.Value
		};
	}

	private static string GetRequiredClaim(ClaimsPrincipal principal, string claimType) =>
			principal.FindFirst(claimType)?.Value ?? throw new InvalidOperationException($"Could not find required '{claimType}' claim.");
}
