using Modules.Shared.Integration.Authorization;
using System.Security.Claims;

namespace Shared.Integration.Extensions;
public static class ClaimPrincipalExtensions
{
	public static bool IsAuthenticated(this ClaimsPrincipal principal)
	{
		return principal?.Identity?.IsAuthenticated == true;
	}

	public static string? GetUserName(this ClaimsPrincipal principal)
	{
		return principal.FindFirst("name")?.Value;
	}

	public static string? GetProfilePicture(this ClaimsPrincipal principal)
	{
		return principal.FindFirst("picture")?.Value;
	}

	public static string? GetEmail(this ClaimsPrincipal principal)
	{
		return principal.FindFirst("email")?.Value;
	}

	public static string? GetUserId(this ClaimsPrincipal principal)
	{
		return principal.FindFirst("sub")?.Value;
	}

	public static Permissions GetPermissions(this ClaimsPrincipal principal)
	{
		var permissionClaim = principal.FindFirst(CustomAuthorizationClaimTypes.Permissions);
		if (!int.TryParse(permissionClaim?.Value, out int permissionClaimValue))
		{
			return Permissions.None;
		}

		return (Permissions)permissionClaimValue;
	}

	public static bool HasPermission(this ClaimsPrincipal principal, Permissions permission)
	{
		var currentPermission = principal.GetPermissions();

		return (currentPermission & permission) != 0;
	}
}
