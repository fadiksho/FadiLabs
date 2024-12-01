using Modules.Auth0.Features.Utils;
using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Querys;

internal class GetUserPermissionsHandler
	(Auth0Api.IManagementApiClient managementApiClient) : IRequestHandler<GetUserPermissions, Result<PagedList<GetRolePermissionResponse>>>
{
	public async Task<Result<PagedList<GetRolePermissionResponse>>> Handle(GetUserPermissions request, CancellationToken cancellationToken)
	{
		var pageInfoRequest = new Auth0Api.Paging.PaginationInfo(request.PageNumber - 1, request.PageSize, true);
		var getUserPermisssoinsRequest = new Auth0Api.Models.UserPermission();

		var clientResponse =
			await managementApiClient.Users.GetPermissionsAsync(request.UserId, pageInfoRequest, cancellationToken);

		var mappedResponse = Map(clientResponse);

		return Result<PagedList<GetRolePermissionResponse>>.FromSuccess(mappedResponse);
	}

	static PagedList<GetRolePermissionResponse> Map(Auth0Api.Paging.IPagedList<Auth0Api.Models.UserPermission> response)
	{
		var mapToDto = response.Select(x => new GetRolePermissionResponse
		{
			Description = x.Description,
			PermissionName = x.Name,
			ResourceServerIdentifier = x.Identifier,
			ResourceServerName = x.ResourceServerName
		}).ToList();

		return Auth0ManagmentApiHelper.ToPagedList(mapToDto, response.Paging);
	}
}
