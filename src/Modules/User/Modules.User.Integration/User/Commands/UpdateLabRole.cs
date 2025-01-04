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

	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var result = await ValidateAsync(ValidationContext<UpdateLabRole>.CreateWithOptions((UpdateLabRole)model, x => x.IncludeProperties(propertyName)));
		if (result.IsValid)
			return [];

		return result.Errors.Select(e => e.ErrorMessage);
	};
}
