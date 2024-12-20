using Microsoft.Extensions.Options;
using Modules.Auth0.Integration.Configuration;
using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Commands;
internal class SetPermissionsHandler
	(Auth0Api.IManagementApiClient managementApiClient,
	IOptions<Auth0Configuration> options) : IRequestHandler<SetPermission, Result>
{
	private readonly Auth0Configuration _auth0Config = options.Value;

	public async Task<Result> Handle(SetPermission request, CancellationToken cancellationToken)
	{
		var updatePermissionRequest = new Auth0Api.Models.ResourceServerUpdateRequest
		{
			Scopes = request.Permissions.Select(x => new Auth0Api.Models.ResourceServerScope
			{
				Description = x.Description,
				Value = x.Value,
			}).ToList()
		};

		await managementApiClient.ResourceServers
			.UpdateAsync(_auth0Config.Audience, updatePermissionRequest, cancellationToken);

		return Result.FromSuccess("Permission Updated");
	}
}
