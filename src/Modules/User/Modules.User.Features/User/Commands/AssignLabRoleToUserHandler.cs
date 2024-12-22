using Microsoft.EntityFrameworkCore;
using Modules.User.Features.Persistence;

namespace Modules.User.Features.User.Commands;

internal class AssignLabRoleToUserHandler
	(IUserContext context) : IRequestHandler<AssignLabRoleToUser, Result>
{
	public async Task<Result> Handle(AssignLabRoleToUser request, CancellationToken cancellationToken)
	{
		var user = await context.LabUsers
			.FirstOrDefaultAsync(x => x.Id == request.LabUserId, cancellationToken);

		var role = await context.LabRoles
			.FirstOrDefaultAsync(x => x.Id == request.LabRoleId, cancellationToken);

		if (user == null || role == null)
			return new NotFoundError();

		if (role.AutoAssign)
			return new InvalidRoleAssignmentError($"The role '{role.Name}' cannot be manually assigned because it is an auto assign role.");

		user.LabRoles.Add(role);
		role.LabUsers.Add(user);

		await context.SaveChangesAsync(cancellationToken);

		return Result.FromSuccess("Operation Success");
	}
}