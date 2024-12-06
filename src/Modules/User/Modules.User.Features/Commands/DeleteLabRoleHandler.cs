using Fadi.Result;
using Fadi.Result.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Modules.User.Features.Persistence;
using Modules.User.Integration;

namespace Modules.User.Features.Commands;
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