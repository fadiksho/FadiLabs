namespace Modules.User.Integration.User.Queries;

[LabAuthorize(LabsPermissions.ConfigureSite)]
public record GetLabUsers : PagedFilterQuery, IRequest<Result<PagedList<GetLabUsersResponse>>>;

public record GetLabUsersResponse
{
	public Guid LabUserId { get; set; }
	public required string Auth0UserId { get; set; }
	public string? DisplayName { get; set; }
	public string? Email { get; set; }
	public bool EmailVerified { get; set; }
	public string? ProfilePictureUrl { get; set; }

	public List<string> Roles { get; set; } = [];
}

public class GetLabUsersValidator : AbstractValidator<GetLabUsers>
{
	public GetLabUsersValidator()
	{
		Include(new PagedFilterQueryValidator());
	}
}
