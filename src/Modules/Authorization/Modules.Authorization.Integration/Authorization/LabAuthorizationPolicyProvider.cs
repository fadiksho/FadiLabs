using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Modules.Shared.Integration.Authorization;

namespace Modules.Authorization.Integration.Authorization;

public class LabAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
	private readonly AuthorizationOptions _options = options.Value;

	public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
	{
		var policy = await base.GetPolicyAsync(policyName);

		if (policy == null && PermissionPolicyManager.IsValidPolicyName(policyName))
		{
			var permissions = PermissionPolicyManager.ExtractPermissions(policyName);

			policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.AddRequirements(new PermissionAuthorizationRequirement(permissions))
					.Build();

			_options.AddPolicy(policyName!, policy);
		}

		return policy;
	}
}
