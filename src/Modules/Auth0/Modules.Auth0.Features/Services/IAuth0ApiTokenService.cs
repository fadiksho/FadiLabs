using Modules.Auth0.Integration.Dtos;

namespace Modules.Auth0.Features.Services;

public interface IAuth0ApiTokenService
{
	Task<AccessTokenResponse> GetAccessTokenAsync(CancellationToken cancellationToken);
}
