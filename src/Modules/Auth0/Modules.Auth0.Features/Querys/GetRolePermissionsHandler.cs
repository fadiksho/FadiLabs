using Modules.Auth0.Features.Utils;
using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Querys;

internal class GetRolePermissionsHandler
	(Auth0Api.IManagementApiClient managementApiClient) : IRequestHandler<GetRolePermission, Result<PagedList<GetRolePermissionResponse>>>
{
	public async Task<Result<PagedList<GetRolePermissionResponse>>> Handle(GetRolePermission request, CancellationToken cancellationToken)
	{
		var pageInfoRequest = new Auth0Api.Paging.PaginationInfo(request.PageNumber - 1, request.PageSize, true);

		var clientResponse = await
			managementApiClient.Roles.GetPermissionsAsync(request.RoleId, pageInfoRequest, cancellationToken);

		var mappedResponse = Map(clientResponse, clientResponse.Paging);

		return Result<PagedList<GetRolePermissionResponse>>.FromSuccess(mappedResponse);
	}

	static PagedList<GetRolePermissionResponse> Map(Auth0Api.Paging.IPagedList<Auth0Api.Models.Permission> clientResponse, Auth0Api.Paging.PagingInformation paging)
	{
		var mapToDto = clientResponse.Select(x => new GetRolePermissionResponse
		{
			PermissionName = x.Name,
			ResourceServerIdentifier = x.Identifier,
			ResourceServerName = x.ResourceServerName,
			Description = x.Description
		}).ToList();

		return Auth0ManagmentApiHelper.ToPagedList(mapToDto, paging);
	}
}
