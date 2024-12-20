using Auth0Api = Auth0.ManagementApi;

namespace Modules.Auth0.Features.Commands;

internal class DeleteUserHandler
	(Auth0Api.IManagementApiClient managementApiClient) : IRequestHandler<DeleteUser, Result>
{
	public async Task<Result> Handle(DeleteUser request, CancellationToken cancellationToken)
	{
		await managementApiClient.Users.DeleteAsync(request.UserId);

		return Result.FromSuccess($"User Deleted.");
	}
}
