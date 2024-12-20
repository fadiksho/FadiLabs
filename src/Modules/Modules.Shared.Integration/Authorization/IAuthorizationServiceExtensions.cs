using Microsoft.AspNetCore.Authorization;
using Modules.Shared.Integration.Authorization;
using System.Security.Claims;

namespace Modules.Shared.Integration.Authorization;

public static class IAuthorizationServiceExtensions
{
	public static Task<AuthorizationResult> AuthorizeAsync(this IAuthorizationService service, ClaimsPrincipal user, LabsPermissions labsPermissions)
	{
		return service.AuthorizeAsync(user, LabsPermissionsPolicyManager.GeneratePolicyName(labsPermissions));
	}
}
