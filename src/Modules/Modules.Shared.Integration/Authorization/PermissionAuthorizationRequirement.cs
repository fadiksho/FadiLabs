using Microsoft.AspNetCore.Authorization;

namespace Modules.Shared.Integration.Authorization;

public class PermissionAuthorizationRequirement(Permissions permission) : IAuthorizationRequirement
{
	public Permissions Permissions { get; } = permission;
}
