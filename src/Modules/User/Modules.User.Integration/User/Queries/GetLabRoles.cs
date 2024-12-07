using Modules.Shared.Integration.Authorization;
using Modules.Shared.Integration.Models;
using Modules.Shared.Integration.Queries;

namespace Modules.User.Integration.User.Queries;

public record GetLabRoles : PagedFilterQuery, IRequest<Result<PagedList<GetLabRolesResponse>>>;

public record GetLabRolesResponse
{
	public Guid Id { get; set; }

	public required string Name { get; set; }
	public required string Description { get; set; }

	public Permissions Permissions { get; set; } = Permissions.None;
}

public class GetLabRolesValidator : AbstractValidator<GetLabRoles>
{
	public GetLabRolesValidator()
	{
		Include(new PagedFilterQueryValidator());
	}
}