using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Modules.Shared.Integration.Authorization;

public class LabAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
	private readonly AuthorizationOptions _options = options.Value;

	public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
	{
		var policy = await base.GetPolicyAsync(policyName);

		if (policy == null && LabsPermissionsPolicyManager.IsValidPolicyName(policyName))
		{
			var labsPermissions = LabsPermissionsPolicyManager.ExtractLabsPermissions(policyName);

			policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.AddRequirements(new PermissionAuthorizationRequirement(labsPermissions))
					.Build();

			_options.AddPolicy(policyName!, policy);
		}

		return policy;
	}
}
