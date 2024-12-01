using Modules.Shared.Integration.Queries;

namespace Modules.Auth0.Integration.Models;
public record GetUsersByRole : PagedFilterQuery, IRequest<Result<PagedList<GetUsersByRoleResponse>>>
{
	public required string RoleId { get; set; }
	public override int PageSize { get; set; } = 50;
}

public record GetUsersByRoleResponse
{
	public required string UserId { get; set; }
	public required string Name { get; set; }
	public required string Picture { get; set; }
	public required string Email { get; set; }
}
