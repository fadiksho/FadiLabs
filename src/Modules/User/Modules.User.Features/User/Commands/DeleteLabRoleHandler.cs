using Microsoft.EntityFrameworkCore;
using Modules.User.Features.Persistence;

namespace Modules.User.Features.User.Commands;
internal class DeleteLabRoleHandler
	(IUserContext context) : IRequestHandler<DeleteLabRole, Result>
{
	public async Task<Result> Handle(DeleteLabRole request, CancellationToken cancellationToken)
	{
		var role = await context.LabRoles
			.FirstOrDefaultAsync(x => x.Id == request.LabRoleId, cancellationToken);

		if (role == null)
			return new NotFoundError();

		context.LabRoles.Remove(role);
		await context.SaveChangesAsync(cancellationToken);

		return Result.FromSuccess("Lab Role Deleted.");
	}
}