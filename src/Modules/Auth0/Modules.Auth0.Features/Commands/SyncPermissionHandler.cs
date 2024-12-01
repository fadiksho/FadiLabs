using Microsoft.Extensions.Options;
using Modules.Auth0.Features.Configuration;
using Modules.Shared.Integration.Authorization;
using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Commands;
internal class SyncPermissionsHandler
	(Auth0Api.IManagementApiClient managementApiClient,
	IOptions<Auth0Configuration> options) : IRequestHandler<SyncPermissions, Result>
{
	private readonly Auth0Configuration auth0Config = options.Value;
	public async Task<Result> Handle(SyncPermissions request, CancellationToken cancellationToken)
	{
		var updatePermissionRequest = new Auth0Api.Models.ResourceServerUpdateRequest
		{
			Scopes = Enum.GetValues<Permissions>()
			.Select(permission => new Auth0Api.Models.ResourceServerScope
			{
				Value = ((int)permission).ToString(),
				Description = permission.ToString()
			}).ToList()
		};

		await managementApiClient.ResourceServers
			.UpdateAsync(auth0Config.Audience, updatePermissionRequest, cancellationToken);

		return Result.FromSuccess("Permissions Synced.");
	}
}

