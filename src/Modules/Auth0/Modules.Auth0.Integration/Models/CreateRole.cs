namespace Modules.Auth0.Integration.Models;
public record CreateRole : IRequest<Result<CreateRoleResponse>>
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	public void ResetToDefault()
	{
		Name = string.Empty;
		Description = string.Empty;
	}
}

public record CreateRoleResponse
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public required string Description { get; set; }
}

public class CreateRoleValidator : AbstractValidator<CreateRole>
{
	public CreateRoleValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty();

		RuleFor(x => x.Description)
			.NotEmpty();
	}
}