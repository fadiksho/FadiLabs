namespace Modules.User.Integration;

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
	public Guid RoleId { get; set; }
}

public class CreateRoleValidator : AbstractValidator<CreateLabRole>
{
	public CreateRoleValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty();

		RuleFor(x => x.Description)
			.NotEmpty();
	}
}