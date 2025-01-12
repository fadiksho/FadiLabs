using System.Security.Claims;

namespace Shared.Features.Server.Services;

public interface ICurrentUser
{
	Task<ClaimsPrincipal> GetUser();
}
