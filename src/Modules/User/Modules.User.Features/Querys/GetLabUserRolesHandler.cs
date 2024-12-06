using Fadi.Result.Errors;
using Microsoft.EntityFrameworkCore;
using Modules.Shared.Integration.Authorization;
using Modules.User.Features.Entities;
using Modules.User.Features.Persistence;

namespace Modules.User.Features.Querys;

internal class GetLabUserRolesHandler
	(IUserContext context) : IRequestHandler<GetLabUserRoles, Result<GetLabUserRolesResponse>>
{
	public async Task<Result<GetLabUserRolesResponse>> Handle(GetLabUserRoles request, CancellationToken cancellationToken)
	{
		var labUserRoles = await context.LabUsers
			.Include(x => x.LabRoles)
			.FirstOrDefaultAsync(x => x.Auth0UserId == request.Auth0UserId, cancellationToken);

		if (labUserRoles == null)
			return new NotFoundError();

		var combindedPermission = Permissions.None;

		foreach (var role in labUserRoles.LabRoles)
			combindedPermission |= role.Permissions;

		var response = Map(labUserRoles, (int)combindedPermission);

		return response;
	}

	static GetLabUserRolesResponse Map(LabUser labUser, int combinedPermissions)
	{
		return new GetLabUserRolesResponse
		{
			CombinedRolesPermissions = combinedPermissions,
			Roles = labUser.LabRoles.Select(x => x.Name).ToList(),
		};
	}
}
