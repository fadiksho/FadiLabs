using Fadi.Result.Errors;
using Microsoft.EntityFrameworkCore;
using Modules.User.Features.Persistence;
using Modules.User.Integration;

namespace Modules.User.Features.Commands;

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

		user.LabRoles.Add(role);
		role.LabUsers.Add(user);

		await context.SaveChangesAsync(cancellationToken);

		return Result.FromSuccess("Operation Success");
	}
}