using Microsoft.EntityFrameworkCore;
using Modules.User.Features.Persistence;

namespace Modules.User.Features.User.Commands;


internal class UnassignLabRoleFromUserHandler
	(IUserContext context) : IRequestHandler<UnassignLabRoleFromUser, Result>
{
	public async Task<Result> Handle(UnassignLabRoleFromUser request, CancellationToken cancellationToken)
	{
		var user = await context.LabUsers
			.FirstOrDefaultAsync(x => x.Id == request.LabUserId, cancellationToken);

		var role = await context.LabRoles
			.FirstOrDefaultAsync(x => x.Id == request.LabRoleId, cancellationToken);

		if (user == null || role == null)
			return new NotFoundError();

		if (role.AutoAssign)
			return new InvalidRoleAssignmentError($"The role '{role.Name}' cannot be manually assigned because it is set to auto assign.");

		user.LabRoles.Remove(role);
		role.LabUsers.Remove(user);

		await context.SaveChangesAsync(cancellationToken);

		return Result.FromSuccess("Operation Success");
	}
}

public record UnassignLabRoleFromUser : IRequest<Result>
{
	public Guid LabRoleId { get; set; }
	public Guid LabUserId { get; set; }
}