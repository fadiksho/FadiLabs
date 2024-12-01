using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Commands;
internal class CreateRoleHandler
	(Auth0Api.IManagementApiClient managementApiClient) : IRequestHandler<CreateRole, Result<CreateRoleResponse>>
{
	public async Task<Result<CreateRoleResponse>> Handle(CreateRole request, CancellationToken cancellationToken)
	{
		var createRoleRequest = new Auth0Api.Models.RoleCreateRequest
		{
			Description = request.Description,
			Name = request.Name
		};

		var clientResponse = await
			managementApiClient.Roles.CreateAsync(createRoleRequest, cancellationToken);

		var mappedResponse = Map(clientResponse);

		return Result<CreateRoleResponse>.FromSuccess(mappedResponse);
	}

	static CreateRoleResponse Map(Auth0Api.Models.Role clientResponse)
	{
		return new CreateRoleResponse
		{
			Id = clientResponse.Id,
			Name = clientResponse.Name,
			Description = clientResponse.Description,
		};
	}
}

