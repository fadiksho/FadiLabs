using Shared.Integration.Services;

namespace Maui.Startup.Services;
internal class MauiTokenService(MauiUserService mauiUserService) : ITokenService
{
	public Task<GetTokensResponse> GetTokens()
	{
		return Task.FromResult(new GetTokensResponse
		{
			IdToken = mauiUserService.IdToken,
			AccessToken = mauiUserService.AccessToken,
			RefreshToken = mauiUserService.RefreshToken,
		});
	}
}
