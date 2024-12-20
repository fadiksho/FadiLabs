using Microsoft.EntityFrameworkCore;
using Modules.Shared.Integration.Authorization;
using Modules.User.Features.Entities;
using Modules.User.Features.Persistence;

namespace Modules.User.Features.User.Querys;

internal class GetLabUserRolesHandler
	(IUserContext context) : IRequestHandler<GetLabUserRoles, Result<GetLabUserRolesResponse>>
{
	public async Task<Result<GetLabUserRolesResponse>> Handle(GetLabUserRoles request, CancellationToken cancellationToken)
	{
		var labUserRoles = await context.LabUsers
			.Include(x => x.LabRoles)
			.FirstOrDefaultAsync(x => x.Auth0UserId == request.Auth0UserId, cancellationToken);

		var defaultRoles = await context.LabRoles
			.Where(x => x.AutoAssign)
			.ToListAsync(cancellationToken);

		List<LabRole> combinedRoles = [.. (labUserRoles?.LabRoles ?? []), .. defaultRoles];

		var combindPermission = LabsPermissions.None;

		foreach (var role in combinedRoles)
			combindPermission |= role.LabsPermissions;

		var response = Map(combinedRoles, (int)combindPermission);

		return response;
	}

	static GetLabUserRolesResponse Map(List<LabRole> roles, int combinedPermissions)
	{
		return new GetLabUserRolesResponse
		{
			CombinedRolesPermissions = combinedPermissions,
			Roles = roles.Select(x => x.Name).ToList(),
		};
	}
}
