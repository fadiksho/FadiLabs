namespace Modules.Shared.Integration.Authorization;

public class LabAuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
	public LabAuthorizeAttribute() { }

	public LabAuthorizeAttribute(string policy) : base(policy) { }

	public LabAuthorizeAttribute(LabsPermissions labsPermissions)
	{
		LabsPermissions = labsPermissions;
	}

	public LabsPermissions LabsPermissions
	{
		get
		{
			return !string.IsNullOrEmpty(Policy)
					? LabsPermissionsPolicyManager.ExtractLabsPermissions(Policy)
					: LabsPermissions.None;
		}
		set
		{
			Policy = value != LabsPermissions.None
					? LabsPermissionsPolicyManager.GeneratePolicyName(value)
					: string.Empty;
		}
	}
}
