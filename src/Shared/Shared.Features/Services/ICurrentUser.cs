using Modules.Shared.Integration.Authorization;
using System.Security.Claims;

namespace Shared.Features.Services;

public interface ICurrentUser
{
	string? GetUserId();
	string? GetUserName();
	ClaimsPrincipal GetUser();
	Permissions GetUserPermissions();


	bool HasPermission(Permissions permission);
}
