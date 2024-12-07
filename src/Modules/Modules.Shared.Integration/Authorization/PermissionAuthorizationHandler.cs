using Microsoft.AspNetCore.Authorization;

namespace Modules.Shared.Integration.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
	{
		if (requirement.LabsPermissions == LabsPermissions.None)
		{
			context.Succeed(requirement);
			return Task.CompletedTask;
		}

		var labsPermissionsClaim = context.User.FindFirst(
				c => c.Type == CustomAuthorizationClaimTypes.LabsPermissions);

		if (labsPermissionsClaim == null)
		{
			return Task.CompletedTask;
		}

		if (!int.TryParse(labsPermissionsClaim.Value, out int labsPermissionsClaimValue))
		{
			return Task.CompletedTask;
		}

		var userPermissions = (LabsPermissions)labsPermissionsClaimValue;

		if ((userPermissions & requirement.LabsPermissions) != 0)
		{
			context.Succeed(requirement);
			return Task.CompletedTask;
		}

		return Task.CompletedTask;
	}
}

