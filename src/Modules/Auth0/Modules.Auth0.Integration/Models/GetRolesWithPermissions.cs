using Modules.Shared.Integration.Authorization;

namespace Modules.Auth0.Integration.Models;
public record GetRolesWithPermissions : IRequest<Result<List<GetRolesWithPermissionsResponse>>>
{

}

public record GetRolesWithPermissionsResponse
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }

	public Permissions Permissions { get; set; }
	public Permissions OriginalPermissions { get; init; }
}
