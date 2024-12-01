using Modules.Auth0.Features.Utils;
using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Querys;

internal class GetRolesHandler(
	Auth0Api.IManagementApiClient managementApiClient) : IRequestHandler<GetRoles, Result<PagedList<GetRolesResponse>>>
{
	public async Task<Result<PagedList<GetRolesResponse>>> Handle(GetRoles request, CancellationToken cancellationToken)
	{
		var getRolesRequest = new Auth0Api.Models.GetRolesRequest();
		var pageInfoRequest = new Auth0Api.Paging.PaginationInfo(request.PageNumber - 1, request.PageSize, true);

		var clientResponse = await
			managementApiClient.Roles.GetAllAsync(getRolesRequest, pageInfoRequest, cancellationToken);

		var mappedResponse = Map(clientResponse);

		return Result<PagedList<GetRolesResponse>>.FromSuccess(mappedResponse);
	}

	static PagedList<GetRolesResponse> Map(Auth0Api.Paging.IPagedList<Auth0Api.Models.Role> response)
	{
		var mapToDto = response.Select(x => new GetRolesResponse
		{
			Id = x.Id,
			Name = x.Name,
			Description = x.Description
		}).ToList();

		return Auth0ManagmentApiHelper.ToPagedList(mapToDto, response.Paging);
	}
}

