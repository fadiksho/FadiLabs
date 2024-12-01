using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Commands;
internal class DeleteRoleHandler
	(Auth0Api.IManagementApiClient managementApiClient) : IRequestHandler<DeleteRole, Result>
{
	public async Task<Result> Handle(DeleteRole request, CancellationToken cancellationToken)
	{
		await managementApiClient.Roles.DeleteAsync(request.RoleId, cancellationToken);

		return Result.FromSuccess($"Role Deleted.");
	}
}
