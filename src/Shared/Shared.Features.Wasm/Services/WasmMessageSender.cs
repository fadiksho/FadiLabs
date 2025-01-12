using Fadi.Result;
using MediatR;
using Shared.Components.Services;
using Shared.Integration.Models;
using Shared.Integration.Services;
using System.Net.Http.Json;

namespace Shared.Features.Wasm.Services;

public class WasmMessageSender(HttpClient httpClient, IEnvelopMessageHandler envelopMessage) : IMessageSender
{
	private readonly string _apiSendGateway = "api/send";
	private readonly HttpClient _httpClient = httpClient;
	private readonly IEnvelopMessageHandler _envelopMessage = envelopMessage;

	public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
		where TResponse : notnull, IResult
	{
		var wrappedRequest = _envelopMessage.Wrap(request);

		var httpResponse = await _httpClient.PostAsJsonAsync(_apiSendGateway, wrappedRequest);

		var messageEnvelop = await httpResponse.Content.ReadFromJsonAsync<EnvelopeMessage>(_envelopMessage.JsonOptions);

		var messageBody = _envelopMessage.UnwrapBody<TResponse>(messageEnvelop!);

		return messageBody;
	}
}
