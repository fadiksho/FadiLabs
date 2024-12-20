using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using Modules.Auth0.Integration.Configuration;
using Modules.Auth0.Integration.Dtos;

namespace Modules.Auth0.Features.Services.Implementaions;

internal class Auth0ApiTokenService(
	HttpClient httpClient,
	HybridCache hybridCache,
	IOptions<Auth0Configuration> config) : IAuth0ApiTokenService
{
	private readonly Auth0Configuration _authConfig = config.Value;
	private static readonly JsonSerializerOptions _jsonOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
	};

	public async Task<AccessTokenResponse> GetAccessTokenAsync(CancellationToken cancellationToken = default)
	{
		return await hybridCache.GetOrCreateAsync("Auth0ApiToken", async cancel =>
		{
			var request = new HttpRequestMessage(HttpMethod.Post, "/oauth/token")
			{
				Content = new FormUrlEncodedContent(
				[
						new("client_id", _authConfig.ClientId),
						new("client_secret", _authConfig.ClientSecret),
						new("audience", $"https://{_authConfig.Domain}/api/v2/"),
						new("grant_type", "client_credentials")
				])
			};

			var httpResponse = await httpClient.SendAsync(request, cancellationToken);
			httpResponse.EnsureSuccessStatusCode();

			await using var responseContentStream = await httpResponse.Content.ReadAsStreamAsync(cancel);
			var accessToken = await JsonSerializer.DeserializeAsync<AccessTokenResponse>
				(responseContentStream, _jsonOptions, cancel);

			ArgumentNullException.ThrowIfNull(accessToken);

			return accessToken;
		}, cancellationToken: cancellationToken);
	}
}
