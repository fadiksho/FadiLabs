using System.Security.Claims;

namespace Modules.Auth0.Features.Utils;
internal static class ClaimsPrincipalExtensions
{
	/// <summary>
	/// Sets the RoleClaimType and NameClaimType for each ClaimsIdentity in the ClaimsPrincipal.
	/// </summary>
	/// <param name="principal">The ClaimsPrincipal to update.</param>
	/// <param name="nameClaimType">The name claim type to set.</param>
	/// <param name="roleClaimType">The role claim type to set.</param>
	/// <returns>A new ClaimsPrincipal with updated claim types.</returns>
	public static ClaimsPrincipal UpdateClaimTypes(this ClaimsPrincipal principal, string nameClaimType, string roleClaimType)
	{
		var updatedIdentities = principal.Identities.Select(identity =>
						new ClaimsIdentity(
								identity.Claims,
								identity.AuthenticationType,
								nameClaimType,
								roleClaimType)).ToList();

		return new ClaimsPrincipal(updatedIdentities);
	}
}
