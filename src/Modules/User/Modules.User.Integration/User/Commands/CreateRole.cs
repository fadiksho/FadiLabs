namespace Modules.User.Integration.User.Commands;

[LabAuthorize(LabsPermissions.ConfigureSite)]
public record CreateLabRole : IRequest<Result<CreateLabRoleResponse>>
{
	public string Name { get; set; } = "";
	public string Description { get; set; } = "";

	public void ResetToDefault()
	{
		Name = string.Empty;
		Description = string.Empty;
	}
}

public record CreateLabRoleResponse
{
	public Guid LabRoleId { get; set; }
}

public class CreateLabRoleValidator : AbstractValidator<CreateLabRole>
{
	public CreateLabRoleValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty();

		RuleFor(x => x.Description)
			.NotEmpty();
	}
}