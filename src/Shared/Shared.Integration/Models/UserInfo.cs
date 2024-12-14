using Modules.Shared.Integration.Authorization;
using System.Data;
using System.Security.Claims;
using static Shared.Integration.SharedConstents;

namespace Shared.Integration.Models;

public sealed class UserInfo
{
	public required string UserId { get; init; }
	public required string Name { get; init; }
	public required string[] Role { get; init; }
	public required string LabsPermissions { get; set; }
	public required string? Email { get; init; }
	public required string Exp { get; set; }

	public const string UserIdClaimType = "sub";
	public const string NameClaimType = LabsClaimTypes.Name;
	public const string EmailClaimType = "email";
	public const string RoleClaimType = LabsClaimTypes.Role;
	public const string LabsPermissionsClaimType = CustomAuthorizationClaimTypes.LabsPermissions;
	public const string ExpClaimType = "exp";
	public static UserInfo BuildUserInfo(ClaimsPrincipal principal)
	{
		return new UserInfo
		{
			UserId = GetRequiredClaim(principal, UserIdClaimType),
			Name = GetRequiredClaim(principal, NameClaimType),
			Role = principal.FindAll(RoleClaimType).Select(c => c.Value).ToArray(),
			LabsPermissions = GetRequiredClaim(principal, LabsPermissionsClaimType),
			Email = principal.FindFirst(EmailClaimType)?.Value,
			Exp = GetRequiredClaim(principal, ExpClaimType)
		};
	}

	public static ClaimsPrincipal BuildClaimsPrincipal(UserInfo userInfo)
	{
		Claim[] claimsToSerialize = [
				new Claim(UserIdClaimType, userInfo.UserId),
				new Claim(NameClaimType, userInfo.Name),
				new Claim(LabsPermissionsClaimType, userInfo.LabsPermissions),
				new Claim(EmailClaimType, userInfo.Email ?? string.Empty),
				new Claim(ExpClaimType, userInfo.Exp),
				.. userInfo.Role.Select(role => new Claim(RoleClaimType, role))
		];

		return new ClaimsPrincipal(
			new ClaimsIdentity(
				claims: claimsToSerialize,
				authenticationType: nameof(UserInfo),
				nameType: NameClaimType,
				roleType: RoleClaimType));
	}

	private static string GetRequiredClaim(ClaimsPrincipal principal, string claimType) =>
			principal.FindFirst(claimType)?.Value ?? throw new InvalidOperationException($"Could not find required '{claimType}' claim.");
}
