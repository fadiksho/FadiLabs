using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Modules.Shared.Integration.Authorization;

namespace Shared.Components.Components.Authorization;

public class LabAuthorizeView : AuthorizeView
{
	//[Parameter]
	//public Permissions LabsPermissions
	//{
	//	get
	//	{
	//		return string.IsNullOrEmpty(Policy)
	//			? LabsPermissions.None
	//			: LabsPermissionPolicyHelper.ExtractLabsPermissions(Policy);
	//	}
	//	set
	//	{
	//		Policy = LabsPermissionsPolicyManager.GeneratePolicyName(value);
	//	}
	//}

	[Parameter]
	public LabsPermissions LabPermissions { get; set; }

	protected override void OnParametersSet()
	{
		Policy = LabsPermissionsPolicyManager.GeneratePolicyName(LabPermissions);
	}
}
