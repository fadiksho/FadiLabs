using Microsoft.EntityFrameworkCore;
using Modules.User.Features.Entities;
using Modules.User.Features.Persistence;
using Modules.User.Features.User.Events;

namespace Modules.User.Features.User.Commands;

internal class CreateLabUserHandler(IUserContext context, IMediator mediator) : IRequestHandler<CreateLabUser, Result<CreateLabUserResponse>>
{
	public async Task<Result<CreateLabUserResponse>> Handle(CreateLabUser request, CancellationToken cancellationToken)
	{
		var userAlreadyExists = await context.LabUsers
			.AnyAsync(x => x.Auth0UserId == request.Auth0UserId, cancellationToken);

		if (userAlreadyExists)
			return new EntityAlreadyExistsError();

		var newUser = MapRequest(request);

		var isFirstUser = await context.LabUsers.AnyAsync(cancellationToken) == false;

		await context.LabUsers.AddAsync(newUser, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		if (isFirstUser)
			await mediator.Publish(new FirstLabUserCreated(newUser.Id), cancellationToken);

		await mediator.Publish(new LabUserCreated(newUser.Id), cancellationToken);

		return MapResponse(newUser);
	}

	static LabUser MapRequest(CreateLabUser request)
	{
		return new LabUser
		{
			Auth0UserId = request.Auth0UserId,
			Email = request.Email,
			ProfilePictureUrl = request.ProfilePictureUrl,
			DisplayName = request.DisplayName,
			EmailVerified = request.EmailVerified
		};
	}

	static CreateLabUserResponse MapResponse(LabUser labUser)
	{
		return new CreateLabUserResponse(labUser.Id);
	}
}
