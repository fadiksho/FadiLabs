using Microsoft.AspNetCore.Authorization;

namespace Modules.Shared.Integration.Authorization;

public class PermissionAuthorizationRequirement(LabsPermissions labsPermissions) : IAuthorizationRequirement
{
	public LabsPermissions LabsPermissions { get; } = labsPermissions;
}
