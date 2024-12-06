using Microsoft.AspNetCore.Authorization;
using Modules.Shared.Integration.Authorization;

namespace Modules.Authorization.Integration.Authorization;

public class PermissionAuthorizationRequirement(Permissions permission) : IAuthorizationRequirement
{
	public Permissions Permissions { get; } = permission;
}
