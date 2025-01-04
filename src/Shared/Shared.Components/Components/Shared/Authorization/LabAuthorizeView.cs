using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Modules.Shared.Integration.Authorization;

namespace Shared.Components.Components.Shared.Authorization;

public class LabAuthorizeView : AuthorizeView
{
	[Parameter]
	public LabsPermissions LabPermissions { get; set; }

	protected override void OnParametersSet()
	{
		Policy = LabsPermissionsPolicyManager.GeneratePolicyName(LabPermissions);
	}
}
