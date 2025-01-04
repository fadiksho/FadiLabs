using Modules.Shared.Integration.Authorization;

namespace Modules.Auth0.Integration.Models;

[LabAuthorize(LabsPermissions.ConfigureSite)]
public record SetPermission : IRequest<Result>
{
	public List<PermissionRequest> Permissions { get; set; } = [];
}

public record PermissionRequest
{
	public string Value { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	public void ResetToDefault()
	{
		Value = string.Empty;
		Description = string.Empty;
	}
}

public class PermissionRequestValidator : AbstractValidator<PermissionRequest>
{
	public PermissionRequestValidator()
	{
		RuleFor(x => x.Value)
			.NotEmpty();

		RuleFor(x => x.Description)
			.NotEmpty();
	}

	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var result = await ValidateAsync(ValidationContext<PermissionRequest>.CreateWithOptions((PermissionRequest)model, x => x.IncludeProperties(propertyName)));
		if (result.IsValid)
			return [];

		return result.Errors.Select(e => e.ErrorMessage);
	};
}