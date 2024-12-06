using Fadi.Result;
using Fadi.Result.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Modules.User.Features.Entities;
using Modules.User.Features.Persistence;
using Modules.User.Integration;

namespace Modules.User.Features.Commands;

internal class CreateLabUserHandler(IUserContext context) : IRequestHandler<CreateLabUser, Result<CreateLabUserResponse>>
{
	public async Task<Result<CreateLabUserResponse>> Handle(CreateLabUser request, CancellationToken cancellationToken)
	{
		var userAlreadyExists = await context.LabUsers
			.AnyAsync(x => x.Auth0UserId == request.Auth0UserId, cancellationToken);

		if (userAlreadyExists)
			return new EntityAlreadyExistsError();

		var newUser = MapRequest(request);

		await context.LabUsers.AddAsync(newUser, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		return MapResponse(newUser);
	}

	static LabUser MapRequest(CreateLabUser request)
	{
		return new LabUser
		{
			Auth0UserId = request.Auth0UserId,
			Email = request.Email,
			ProfilePictureUrl = request.ProfilePictureUrl,
		};
	}

	static CreateLabUserResponse MapResponse(LabUser labUser)
	{
		return new CreateLabUserResponse(labUser.Id);
	}
}
