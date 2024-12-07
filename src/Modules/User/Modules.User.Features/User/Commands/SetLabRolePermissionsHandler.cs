using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modules.Shared.Integration.Authorization;
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

		role.Permissions = request.UpdatedPermissions;

		context.LabRoles.Update(role);
		await context.SaveChangesAsync(cancellationToken);

		return Result.FromSuccess("Operation success.");
	}
}

public record SetLabRolePermissions : IRequest<Result>
{
	public Guid LabRoleId { get; set; }

	public Permissions UpdatedPermissions { get; set; } = Permissions.None;
}

public class SetLabRolePermissionsValidator : AbstractValidator<SetLabRolePermissions>
{
	public SetLabRolePermissionsValidator()
	{
		RuleFor(x => x.LabRoleId)
			.NotEmpty();
	}
}