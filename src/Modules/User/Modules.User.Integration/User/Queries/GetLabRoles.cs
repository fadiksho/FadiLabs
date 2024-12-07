namespace Modules.User.Integration.User.Queries;

[LabAuthorize(LabsPermissions.ConfigureSite)]
public record GetLabRoles : PagedFilterQuery, IRequest<Result<PagedList<GetLabRolesResponse>>>;

public record GetLabRolesResponse
{
	public Guid Id { get; set; }

	public required string Name { get; set; }
	public required string Description { get; set; }

	public LabsPermissions LabsPermissions { get; set; } = LabsPermissions.None;
}

public class GetLabRolesValidator : AbstractValidator<GetLabRoles>
{
	public GetLabRolesValidator()
	{
		Include(new PagedFilterQueryValidator());
	}
}