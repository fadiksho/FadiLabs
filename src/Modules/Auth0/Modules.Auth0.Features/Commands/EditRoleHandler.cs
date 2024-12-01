using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Commands;
internal class EditRoleHandler
	(Auth0Api.IManagementApiClient managementApiClient) : IRequestHandler<EditRole, Result<EditRoleResponse>>
{
	public async Task<Result<EditRoleResponse>> Handle(EditRole request, CancellationToken cancellationToken)
	{
		var editRoleRequest = new Auth0Api.Models.RoleUpdateRequest
		{
			Name = request.Name,
			Description = request.Description
		};

		var clientResponse = await
			managementApiClient.Roles.UpdateAsync(request.Id, editRoleRequest, cancellationToken);

		var mappedResponse = Map(clientResponse);

		return Result<EditRoleResponse>.FromSuccess(mappedResponse);
	}

	static EditRoleResponse Map(Auth0Api.Models.Role clientResponse)
	{
		return new EditRoleResponse
		{
			Id = clientResponse.Id,
			Name = clientResponse.Name,
			Description = clientResponse.Description,
		};
	}
}
