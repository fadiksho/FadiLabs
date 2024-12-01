﻿namespace Modules.Auth0.Integration.Models;

public record SetPermission : IRequest<Result>
{
	public List<PermissionRequest> Permissions { get; set; } = [];
}

public class PermissionRequest
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
}