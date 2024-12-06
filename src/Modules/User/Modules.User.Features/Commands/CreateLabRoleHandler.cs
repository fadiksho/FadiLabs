using Modules.User.Features.Entities;
using Modules.User.Features.Persistence;

namespace Modules.User.Features.Commands;

internal class CreateLabRoleHandler
	(IUserContext context) : IRequestHandler<CreateLabRole, Result<CreateLabRoleResponse>>
{
	public async Task<Result<CreateLabRoleResponse>> Handle(CreateLabRole request, CancellationToken cancellationToken)
	{
		var newLab = MapRequest(request);

		await context.LabRoles.AddAsync(newLab, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		return MapResponse(newLab);
	}

	static LabRole MapRequest(CreateLabRole labRole)
	{
		return new LabRole
		{
			Name = labRole.Name,
			Description = labRole.Description
		};
	}

	static CreateLabRoleResponse MapResponse(LabRole labRole)
	{
		return new CreateLabRoleResponse
		{
			LabRoleId = labRole.Id
		};
	}
}
