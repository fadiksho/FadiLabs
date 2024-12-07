using Microsoft.Extensions.Options;
using Modules.Auth0.Integration.Configuration;
using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Querys;

internal class GetPermissionsHandler
	(Auth0Api.IManagementApiClient managementApiClient,
	IOptions<Auth0Configuration> options) : IRequestHandler<GetPermissions, Result<GetPermissionsResponse>>
{
	private readonly Auth0Configuration _auth0Config = options.Value;

	public async Task<Result<GetPermissionsResponse>> Handle(GetPermissions request, CancellationToken cancellationToken)
	{
		var pageInfoRequest =
			new Auth0Api.Paging.PaginationInfo(request.PageNumber - 1, request.PageSize, true);

		var clientResponse = await
			managementApiClient.ResourceServers.GetAsync(_auth0Config.Audience, cancellationToken);

		var mappedResponse = Map(clientResponse);

		return Result<GetPermissionsResponse>.FromSuccess(mappedResponse);
	}

	static GetPermissionsResponse Map(Auth0Api.Models.ResourceServer clientResponse)
	{
		return new GetPermissionsResponse
		{
			Name = clientResponse.Name,
			Permission = clientResponse.Scopes.Select(x => new PermissionResponse
			{
				Value = x.Value,
				Description = x.Description,
			}).ToList()
		};
	}
}

