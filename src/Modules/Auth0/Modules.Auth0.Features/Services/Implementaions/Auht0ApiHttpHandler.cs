using System.Net.Http.Headers;

namespace Modules.Auth0.Features.Services.Implementaions;
internal class Auht0ApiHttpHandler
	(IAuth0ApiTokenService auth0ApiTokenService) : DelegatingHandler
{
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var accessToken = await auth0ApiTokenService.GetAccessTokenAsync(cancellationToken);
		request.Headers.Authorization =
				new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

		request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

		var responseMessage = await base.SendAsync(request, cancellationToken);

		return responseMessage;
	}
}
