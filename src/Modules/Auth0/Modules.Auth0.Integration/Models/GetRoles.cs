using Modules.Shared.Integration.Queries;

namespace Modules.Auth0.Integration.Models;
public record GetRoles : PagedFilterQuery, IRequest<Result<PagedList<GetRolesResponse>>>
{
	public override int PageSize { get; set; } = 50;
}

public record GetRolesResponse
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public required string Description { get; set; }
}