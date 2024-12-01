using Modules.Auth0.Features.Utils;
using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Querys;

internal class GetUsersHandler(Auth0Api.IManagementApiClient managementApiClient) : IRequestHandler<GetUsers, Result<PagedList<GetUsersResponse>>>
{
	public async Task<Result<PagedList<GetUsersResponse>>> Handle(GetUsers request, CancellationToken cancellationToken)
	{
		var getUserRequest = new Auth0Api.Models.GetUsersRequest
		{
			Fields = $"name,user_id,email,email_verified,picture,blocked",
			IncludeFields = true
		};
		var pageInfoRequest = new Auth0Api.Paging.PaginationInfo(0, 50, true);

		var clientResponse = await
			managementApiClient.Users.GetAllAsync(getUserRequest, pageInfoRequest, cancellationToken);

		var mappedResponse = Map(clientResponse, clientResponse.Paging);

		return Result<PagedList<GetUsersResponse>>.FromSuccess(mappedResponse);
	}

	static PagedList<GetUsersResponse> Map(Auth0Api.Paging.IPagedList<Auth0Api.Models.User> response, Auth0Api.Paging.PagingInformation pagingInfo)
	{
		var mapToDto = response.Select(x => new GetUsersResponse
		{
			UserId = x.UserId,
			Name = x.FullName,
			Picture = x.Picture,
			Email = x.Email,
			EmailVerified = x.EmailVerified,
			Blocked = x.Blocked ?? false,
		}).ToList();

		return Auth0ManagmentApiHelper.ToPagedList(mapToDto, pagingInfo);
	}
}
