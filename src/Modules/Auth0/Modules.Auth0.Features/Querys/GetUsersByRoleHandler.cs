using Auth0.ManagementApi.Models;
using Modules.Auth0.Features.Utils;
using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Querys;

internal class GetUsersByRoleHandler
	(Auth0Api.IManagementApiClient managementApiClient) : IRequestHandler<GetUsersByRole, Result<PagedList<GetUsersByRoleResponse>>>
{
	public async Task<Result<PagedList<GetUsersByRoleResponse>>> Handle(GetUsersByRole request, CancellationToken cancellationToken)
	{
		var pageInfoRequest = new Auth0Api.Paging.PaginationInfo(request.PageNumber - 1, request.PageSize, true);

		var clientResponse = await
			managementApiClient.Roles.GetUsersAsync(request.RoleId, pageInfoRequest, cancellationToken);

		var mappedResponse = Map(clientResponse, clientResponse.Paging);

		return Result<PagedList<GetUsersByRoleResponse>>.FromSuccess(mappedResponse);
	}

	static PagedList<GetUsersByRoleResponse> Map(Auth0Api.Paging.IPagedList<AssignedUser> clientResponse, Auth0Api.Paging.PagingInformation paging)
	{
		var mapToDto = clientResponse.Select(x => new GetUsersByRoleResponse
		{
			UserId = x.UserId,
			Email = x.Email,
			Name = x.FullName,
			Picture = x.Picture,
		}).ToList();

		return Auth0ManagmentApiHelper.ToPagedList(mapToDto, paging);
	}
}
