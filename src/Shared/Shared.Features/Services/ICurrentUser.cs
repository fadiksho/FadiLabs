using System.Security.Claims;

namespace Shared.Features.Services;

public interface ICurrentUser
{
	Task<ClaimsPrincipal> GetUser();
}
