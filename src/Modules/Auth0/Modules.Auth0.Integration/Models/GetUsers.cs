using Modules.Shared.Integration.Authorization;

namespace Modules.Auth0.Integration.Models;

[LabAuthorize(LabsPermissions.ConfigureSite)]
public record GetUsers : IRequest<Result<PagedList<GetUsersResponse>>>
{

}

public record GetUsersResponse
{
	public required string UserId { get; set; }
	public required string Name { get; set; }
	public required string Picture { get; set; }
	public required string Email { get; set; }
	public bool? EmailVerified { get; set; }
	public bool Blocked { get; set; }
}
