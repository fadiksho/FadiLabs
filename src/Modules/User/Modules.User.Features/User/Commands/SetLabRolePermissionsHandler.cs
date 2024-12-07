using Microsoft.EntityFrameworkCore;
using Modules.User.Features.Persistence;

namespace Modules.User.Features.User.Commands;

internal class SetLabRolePermissionsHandler
	(IUserContext context) : IRequestHandler<SetLabRolePermissions, Result>
{
	public async Task<Result> Handle(SetLabRolePermissions request, CancellationToken cancellationToken)
	{
		var role = await context.LabRoles
			.FirstOrDefaultAsync(x => x.Id == request.LabRoleId, cancellationToken);

		if (role == null)
			return new NotFoundError();

		role.LabsPermissions = request.UpdatedPermissions;

		context.LabRoles.Update(role);
		await context.SaveChangesAsync(cancellationToken);

		return Result.FromSuccess("Operation success.");
	}
}
