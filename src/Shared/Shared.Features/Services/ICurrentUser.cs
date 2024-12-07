using Modules.Shared.Integration.Authorization;
using System.Security.Claims;

namespace Shared.Features.Services;

public interface ICurrentUser
{
	ClaimsPrincipal GetUser();
	LabsPermissions GetUserPermissions();


	bool HasLabPermission(LabsPermissions permission);
}
