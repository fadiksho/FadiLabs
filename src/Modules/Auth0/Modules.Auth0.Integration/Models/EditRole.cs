namespace Modules.Auth0.Integration.Models;
public record EditRole : IRequest<Result<EditRoleResponse>>
{
	public string Id { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	public void ResetToDefault()
	{
		Id = string.Empty;
		Name = string.Empty;
		Description = string.Empty;
	}
}

public record EditRoleResponse
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public required string Description { get; set; }
}

public class EditRoleValidator : AbstractValidator<EditRole>
{
	public EditRoleValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();

		RuleFor(x => x.Name)
			.NotEmpty();

		RuleFor(x => x.Description)
			.NotEmpty();
	}
}