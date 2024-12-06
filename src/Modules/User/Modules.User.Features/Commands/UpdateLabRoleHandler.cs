using Microsoft.EntityFrameworkCore;
using Modules.User.Features.Persistence;

namespace Modules.User.Features.Commands;
internal class UpdateLabRoleHandler(IUserContext context) : IRequestHandler<UpdateLabRole, Result>
{
	public async Task<Result> Handle(UpdateLabRole request, CancellationToken cancellationToken)
	{
		var role = await context.LabRoles
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (role == null)
			return new NotFoundError();

		role.Name = request.Name;
		role.Description = request.Description;

		context.LabRoles.Update(role);
		await context.SaveChangesAsync(cancellationToken);

		return Result.FromSuccess("Operation success");
	}
}
