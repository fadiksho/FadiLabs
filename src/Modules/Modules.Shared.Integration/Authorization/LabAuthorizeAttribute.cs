namespace Modules.Shared.Integration.Authorization;

public class LabAuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
	public LabAuthorizeAttribute() { }

	public LabAuthorizeAttribute(string policy) : base(policy) { }

	public LabAuthorizeAttribute(Permissions permission)
	{
		Permissions = permission;
	}

	public Permissions Permissions
	{
		get
		{
			return !string.IsNullOrEmpty(Policy)
					? PermissionPolicyManager.ExtractPermissions(Policy)
					: Permissions.None;
		}
		set
		{
			Policy = value != Permissions.None
					? PermissionPolicyManager.GeneratePolicyName(value)
					: string.Empty;
		}
	}
}
