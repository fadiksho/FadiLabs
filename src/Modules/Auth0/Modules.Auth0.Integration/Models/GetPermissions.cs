using Modules.Shared.Integration.Authorization;
using Modules.Shared.Integration.Queries;

namespace Modules.Auth0.Integration.Models;

[LabAuthorize(LabsPermissions.ConfigureSite)]
public record GetPermissions : PagedFilterQuery, IRequest<Result<GetPermissionsResponse>>
{
	public override int PageSize { get; set; } = 50;
}

public record GetPermissionsResponse
{
	public required string Name { get; set; }
	public List<PermissionResponse> Permission { get; set; } = [];
}

public record PermissionResponse
{
	public required string Value { get; set; }
	public required string Description { get; set; }
}
