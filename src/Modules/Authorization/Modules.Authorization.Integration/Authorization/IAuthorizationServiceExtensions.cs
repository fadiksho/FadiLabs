using Microsoft.AspNetCore.Authorization;
using Modules.Shared.Integration.Authorization;
using System.Security.Claims;

namespace Modules.Authorization.Integration.Authorization;

public static class IAuthorizationServiceExtensions
{
	public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user, Permissions permissions)
	{
		return service.AuthorizeAsync(user, PermissionPolicyManager.GeneratePolicyName(permissions));
	}
}
