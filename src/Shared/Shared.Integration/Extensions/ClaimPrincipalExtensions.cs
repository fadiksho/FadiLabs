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

	public static LabsPermissions GetPermissions(this ClaimsPrincipal principal)
	{
		var permissionClaim = principal.FindFirst(CustomAuthorizationClaimTypes.LabsPermissions);
		if (!int.TryParse(permissionClaim?.Value, out int permissionClaimValue))
		{
			return LabsPermissions.None;
		}

		return (LabsPermissions)permissionClaimValue;
	}

	/// <summary>
	/// Checks if the specified permission(s) are set in the user's permissions.
	/// </summary>
	/// <param name="principal">The user's claims principal.</param>
	/// <param name="requiredLabPermission">The permission(s) to check.</param>
	/// <returns>True if the specified permission(s) are set; otherwise, false.</returns>
	/// <remarks>
	/// This method checks if the user has the specified permission(s). 
	/// If the permission to check is 'All', it verifies if the user has 'All' permissions.
	/// 
	/// Otherwise, it uses a bitwise AND operation to check if the user has the specified permission(s).
	/// </remarks>
	public static bool HasLabPermission(this ClaimsPrincipal principal, LabsPermissions requiredLabPermission)
	{
		if (requiredLabPermission == LabsPermissions.None)
			return true;

		// Retrieve the current permissions of the user.
		var currentPermission = principal.GetPermissions();

		if (requiredLabPermission == LabsPermissions.All)
			return currentPermission == LabsPermissions.All;

		// Use bitwise AND to check if the user has the specified permission(s).
		return (currentPermission & requiredLabPermission) != 0;
	}

}
