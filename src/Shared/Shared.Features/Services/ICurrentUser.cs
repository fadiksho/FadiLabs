using Modules.Shared.Integration.Authorization;
using System.Security.Claims;

namespace Shared.Features.Services;

public interface ICurrentUser
{
	ClaimsPrincipal GetUser();
	Permissions GetUserPermissions();


	bool HasPermission(Permissions permission);
}
