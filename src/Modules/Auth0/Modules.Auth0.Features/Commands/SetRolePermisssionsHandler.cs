using Microsoft.Extensions.Options;
using Modules.Auth0.Features.Configuration;
using Modules.Shared.Integration.Authorization;
using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Commands;

internal class SetRolePermisssionsHandler
	(Auth0Api.IManagementApiClient managementApiClient,
	IOptions<Auth0Configuration> options) : IRequestHandler<SetRolePermissions, Result>
{
	private readonly Auth0Configuration auth0Config = options.Value;
	public async Task<Result> Handle(SetRolePermissions request, CancellationToken cancellationToken)
	{
		var addedPermissions = request.UpdatedPermissions & ~request.OriginalPermissions;
		var removedPermissions = request.OriginalPermissions & ~request.UpdatedPermissions;

		var removedPermissionsRequest = new Auth0Api.Models.AssignPermissionsRequest()
		{
			Permissions = []
		};

		var assignPermissionsRequest = new Auth0Api.Models.AssignPermissionsRequest()
		{
			Permissions = []
		};

		foreach (Permissions permission in Enum.GetValues<Permissions>())
		{
			if (addedPermissions.HasFlag(permission) && permission != Permissions.None)
			{
				assignPermissionsRequest.Permissions.Add(new Auth0Api.Models.PermissionIdentity
				{
					Identifier = auth0Config.Audience,
					Name = ((int)permission).ToString()
				});
			}
			if (removedPermissions.HasFlag(permission) && permission != Permissions.None)
			{
				removedPermissionsRequest.Permissions.Add(new Auth0Api.Models.PermissionIdentity
				{
					Identifier = auth0Config.Audience,
					Name = ((int)permission).ToString()
				});
			}
		}

		if (assignPermissionsRequest.Permissions.Count > 0)
		{
			await managementApiClient.Roles
				.AssignPermissionsAsync(request.RoleId, assignPermissionsRequest, cancellationToken);
		}
		if (removedPermissionsRequest.Permissions.Count > 0)
		{
			await managementApiClient.Roles
				.RemovePermissionsAsync(request.RoleId, removedPermissionsRequest, cancellationToken);
		}

		return Result.FromSuccess("Role permissions updated");
	}
}

