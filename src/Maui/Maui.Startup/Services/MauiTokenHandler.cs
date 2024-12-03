using System.Net.Http.Headers;

namespace Maui.Startup.Services;
public class TokenHandler
	(MauiUserService userService) : DelegatingHandler
{
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		request.Headers.Authorization =
				new AuthenticationHeaderValue("Bearer", userService.AccessToken);

		var responseMessage = await base.SendAsync(request, cancellationToken);

		return responseMessage;
	}
}
