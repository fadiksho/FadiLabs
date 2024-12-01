using Microsoft.AspNetCore.Authorization;
using Modules.Shared.Integration.Authorization;

namespace Modules.Authorization.Integration.Authorization;

public class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
	public PermissionAuthorizationRequirement(Permissions permission)
	{
		Permissions = permission;
	}

	public Permissions Permissions { get; }
}
