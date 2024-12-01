using Modules.Shared.Integration.Models;
using Modules.Shared.Integration.Queries;

namespace Modules.Auth0.Integration.Models;
public record GetRolePermission : PagedFilterQuery, IRequest<Result<PagedList<GetRolePermissionResponse>>>
{
	public required string RoleId { get; set; }
	public override int PageSize { get; set; } = 50;
}

public record GetRolePermissionResponse
{
	public required string ResourceServerIdentifier { get; set; }
	public required string PermissionName { get; set; }
	public required string ResourceServerName { get; set; }
	public string? Description { get; set; }
}