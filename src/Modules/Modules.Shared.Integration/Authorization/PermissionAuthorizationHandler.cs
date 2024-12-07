using Microsoft.AspNetCore.Authorization;

namespace Modules.Shared.Integration.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
	{
		if (requirement.Permissions == Permissions.None)
		{
			context.Succeed(requirement);
			return Task.CompletedTask;
		}

		var permissionClaim = context.User.FindFirst(
				c => c.Type == CustomAuthorizationClaimTypes.Permissions);

		if (permissionClaim == null)
		{
			return Task.CompletedTask;
		}

		if (!int.TryParse(permissionClaim.Value, out int permissionClaimValue))
		{
			return Task.CompletedTask;
		}

		var userPermissions = (Permissions)permissionClaimValue;

		if ((userPermissions & requirement.Permissions) != 0)
		{
			context.Succeed(requirement);
			return Task.CompletedTask;
		}

		return Task.CompletedTask;
	}
}

