using Modules.Shared.Integration.Authorization;

namespace Modules.Auth0.Integration.Models;
public class SetRolePermissions : IRequest<Result>
{
	public required string RoleId { get; set; }

	public Permissions OriginalPermissions { get; set; }
	public Permissions UpdatedPermissions { get; set; }
}