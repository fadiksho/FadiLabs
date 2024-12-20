using Microsoft.AspNetCore.Authorization;

namespace Modules.User.Integration.User.Queries;

//[LabAuthorize(LabsPermissions.ConfigureSite)]
[Authorize(Policy = LabPolicyNames.ActionTiggerPolicy)]
public record GetLabUserRoles : IRequest<Result<GetLabUserRolesResponse>>
{
	public required string Auth0UserId { get; set; }
}

public record GetLabUserRolesResponse
{
	public List<string> Roles { get; set; } = [];

	public int CombinedRolesPermissions { get; set; }
}