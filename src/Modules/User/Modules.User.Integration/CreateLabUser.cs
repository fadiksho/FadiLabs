using Microsoft.AspNetCore.Authorization;
using Modules.Shared.Integration.Authorization;

namespace Modules.User.Integration;

[Authorize(LabPolicyNames.ActionTiggerPolicy)]
public record CreateLabUser() : IRequest<Result<CreateLabUserResponse>>
{
	public required string Auth0UserId { get; set; }
	public string? DisplayName { get; set; }
	public string? Email { get; set; }
	public bool EmailVerified { get; set; }
	public string? ProfilePictureUrl { get; set; }
}

public record CreateLabUserResponse(Guid UserId);

public class CreateUserValidator : AbstractValidator<CreateLabUser>
{
	public CreateUserValidator()
	{
		RuleFor(x => x.Auth0UserId)
			.NotEmpty();
	}
}