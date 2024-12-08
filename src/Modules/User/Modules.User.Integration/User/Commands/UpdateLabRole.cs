namespace Modules.User.Integration.User.Commands;

[LabAuthorize(LabsPermissions.ConfigureSite)]
public record UpdateLabRole : IRequest<Result>
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public bool AutoAssign { get; set; }

	public void ResetToDefault()
	{
		Name = string.Empty;
		Description = string.Empty;
		AutoAssign = false;
	}
}

public class UpdateLabRoleValidator : AbstractValidator<UpdateLabRole>
{
	public UpdateLabRoleValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty();

		RuleFor(x => x.Description)
			.NotEmpty();
	}
}
