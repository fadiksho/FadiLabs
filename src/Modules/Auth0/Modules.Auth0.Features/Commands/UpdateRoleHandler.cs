using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Commands;
internal class UpdateRoleHandler(
	Auth0Api.IManagementApiClient managementApiClient) : IRequestHandler<UpdateRole, Result>
{
	public async Task<Result> Handle(UpdateRole request, CancellationToken cancellationToken)
	{
		var updateRoleRequest = new Auth0Api.Models.RoleUpdateRequest
		{
			Description = request.Description,
			Name = request.Name
		};

		await managementApiClient.Roles.UpdateAsync(request.RoleId, updateRoleRequest, cancellationToken);

		return Result.FromSuccess("Role Updated.");
	}
}

