namespace Modules.User.Integration.User.Commands;

[LabAuthorize(LabsPermissions.ConfigureSite)]
public record SetLabRolePermissions : IRequest<Result>
{
	public Guid LabRoleId { get; set; }

	public LabsPermissions UpdatedPermissions { get; set; } = LabsPermissions.None;
}

public class SetLabRolePermissionsValidator : AbstractValidator<SetLabRolePermissions>
{
	public SetLabRolePermissionsValidator()
	{
		RuleFor(x => x.LabRoleId)
			.NotEmpty();
	}
}