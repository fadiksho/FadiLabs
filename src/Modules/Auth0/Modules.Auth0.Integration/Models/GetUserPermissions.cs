using Modules.Shared.Integration.Queries;

namespace Modules.Auth0.Integration.Models;
public record GetUserPermissions : PagedFilterQuery, IRequest<Result<PagedList<GetRolePermissionResponse>>>
{
	public override int PageSize { get; set; } = 50;

	public required string UserId { get; set; }
}