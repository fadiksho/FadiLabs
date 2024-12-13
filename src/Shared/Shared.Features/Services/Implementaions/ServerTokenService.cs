using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Shared.Integration.Services;
using System.Diagnostics;

namespace Shared.Features.Services.Implementaions;

public class ServerTokenService
	(IHttpContextAccessor httpContextAccessor) : ITokenService
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	public async Task<GetTokensResponse> GetTokens()
	{
		var idToken = await GetHttpContext().GetTokenAsync("id_token");
		var accessToken = await GetHttpContext().GetTokenAsync("access_token");
		var refreshToken = await GetHttpContext().GetTokenAsync("refresh_token");

		return new GetTokensResponse
		{
			IdToken = idToken,
			AccessToken = accessToken,
			RefreshToken = refreshToken,
		};
	}

	private HttpContext GetHttpContext()
	{
		if (_httpContextAccessor.HttpContext == null)
			throw new UnreachableException($"HttpContext is not set in {nameof(ServerCurrentUser)}().");

		return _httpContextAccessor.HttpContext;
	}
}
