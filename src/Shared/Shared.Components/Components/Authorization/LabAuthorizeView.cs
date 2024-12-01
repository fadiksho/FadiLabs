using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Modules.Shared.Integration.Authorization;

namespace Shared.Components.Components.Authorization;

public class LabAuthorizeView : AuthorizeView
{
	//[Parameter]
	//public Permissions Permissions
	//{
	//	get
	//	{
	//		return string.IsNullOrEmpty(Policy)
	//			? Permissions.None
	//			: PermissionPolicyHelper.GetPermissionsFromPolicyName(Policy);
	//	}
	//	set
	//	{
	//		Policy = PermissionPolicyHelper.GeneratePolicyName(value);
	//	}
	//}

	[Parameter]
	public Permissions Permissions { get; set; }

	protected override void OnParametersSet()
	{
		Policy = PermissionPolicyManager.GeneratePolicyName(Permissions);
	}
}
