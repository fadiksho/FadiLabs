
using Modules.Shared.Integration.Authorization;
using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Querys;

internal class GetRolesWithPermissionsHandler(
	Auth0Api.IManagementApiClient managementApiClient) : IRequestHandler<GetRolesWithPermissions, Result<List<GetRolesWithPermissionsResponse>>>
{
	public async Task<Result<List<GetRolesWithPermissionsResponse>>> Handle(GetRolesWithPermissions request, CancellationToken cancellationToken)
	{
		var getRolesRequest = new Auth0Api.Models.GetRolesRequest();
		var pageInfoRequest = new Auth0Api.Paging.PaginationInfo(0, 50, true);

		var clientResponse = await
			managementApiClient.Roles.GetAllAsync(getRolesRequest, pageInfoRequest, cancellationToken);

		List<GetRolesWithPermissionsResponse> rolesWithPermissions = [];
		foreach (var role in clientResponse)
		{
			var rolePermissions = await managementApiClient.Roles.GetPermissionsAsync(role.Id, pageInfoRequest, cancellationToken);
			var roleWithPermissionResponse = Map(role, rolePermissions);
			rolesWithPermissions.Add(roleWithPermissionResponse);
		}

		return rolesWithPermissions;
	}

	static GetRolesWithPermissionsResponse Map(Auth0Api.Models.Role role, Auth0Api.Paging.IPagedList<Auth0Api.Models.Permission> permissions)
	{
		var rolePermission = PermissionPolicyManager.ConvertToLabsPermissions(permissions.Select(x => x.Name));
		var mapToDto = new GetRolesWithPermissionsResponse
		{
			Id = role.Id,
			Name = role.Name,
			Description = role.Description,
			Permissions = rolePermission,
			OriginalPermissions = rolePermission
		};

		return mapToDto;
	}
}