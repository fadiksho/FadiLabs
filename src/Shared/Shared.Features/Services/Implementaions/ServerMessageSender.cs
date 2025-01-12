using MediatR;
using Shared.Components.Services;

namespace Shared.Features.Services.Implementaions;

internal class ServerMessageSender(IMediator mediator) : IMessageSender
{
	public Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
		where TResponse : notnull, Fadi.Result.IResult
	{
		return mediator.Send(request);
	}
}

// Ops fetch request instead!
//internal class ServerHttpMessageSender(IHttpClientFactory httpClientFactory, IEnvelopMessageHandler envelopMessage) : IMessageSender
//{
//	private readonly string _apiSendGateway = "api/send";
//	private readonly HttpClient _httpClient = httpClientFactory.CreateClient(nameof(ServerHttpMessageSender));
//	private readonly IEnvelopMessageHandler _envelopMessage = envelopMessage;

//	public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
//		where TResponse : notnull, Fadi.Result.IResult
//	{
//		var wrappedRequest = _envelopMessage.Wrap(request);

//		var httpRequest = new HttpRequestMessage(HttpMethod.Post, _apiSendGateway)
//		{
//			Content = JsonContent.Create(wrappedRequest, options: _envelopMessage.JsonOptions),
//		};
//		httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

//		var httpResponse = await _httpClient.SendAsync(httpRequest);

//		httpResponse.EnsureSuccessStatusCode();

//		var messageEnvelop = await httpResponse.Content.ReadFromJsonAsync<EnvelopeMessage>(_envelopMessage.JsonOptions);

//		var messageBody = _envelopMessage.UnwrapBody<TResponse>(messageEnvelop!);

//		return messageBody;
//	}
//}

//internal class ServerHttpMessageSenderHandler : DelegatingHandler
//{
//	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//	{
//		request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

//		return base.SendAsync(request, cancellationToken);
//	}
//}