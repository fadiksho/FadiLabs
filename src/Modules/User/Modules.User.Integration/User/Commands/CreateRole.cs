namespace Modules.User.Integration.User.Commands;

[LabAuthorize(LabsPermissions.ConfigureSite)]
public record CreateLabRole : IRequest<Result<CreateLabRoleResponse>>
{
	public string Name { get; set; } = "";
	public string Description { get; set; } = "";
	public bool AutoAssign { get; set; }

	public void ResetToDefault()
	{
		Name = string.Empty;
		Description = string.Empty;
		AutoAssign = false;
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

	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var result = await ValidateAsync(ValidationContext<CreateLabRole>.CreateWithOptions((CreateLabRole)model, x => x.IncludeProperties(propertyName)));
		if (result.IsValid)
			return [];

		return result.Errors.Select(e => e.ErrorMessage);
	};
}