using Fadi.Result;
using Fadi.Result.Errors;
using MediatR;
using Shared.Components.Services;
using Shared.Integration.Models;
using Shared.Integration.Services;
using System.Net.Http.Json;

namespace Maui.Startup.Services;

public class MauiMessageSender(
  HttpClient httpClient,
  IEnvelopMessageHandler envelopMessage,
  MauiUserService mauiUserService) : IMessageSender
{
  private readonly string _apiSendGateway = "api/send";
  private readonly HttpClient _httpClient = httpClient;
  private readonly IEnvelopMessageHandler _envelopMessage = envelopMessage;
  private readonly MauiUserService _mauiUserService = mauiUserService;

  public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    where TResponse : notnull, IResult
  {
    async Task<TResponse> doSendAsync()
    {
      var wrappedRequest = _envelopMessage.Wrap(request);
      var httpResponse = await _httpClient.PostAsJsonAsync(_apiSendGateway, wrappedRequest);
      var messageEnvelop = await httpResponse.Content.ReadFromJsonAsync<EnvelopeMessage>(_envelopMessage.JsonOptions);
      var messageBody = _envelopMessage.UnwrapBody<TResponse>(messageEnvelop!);

      return messageBody;
    }

    var messageResult = await doSendAsync();

    if (!messageResult.IsSuccess
      && messageResult.Error is UnauthentectedError
      && !string.IsNullOrEmpty(_mauiUserService.RefreshToken))
    {
      await _mauiUserService.RefreshTokenAsync();
      messageResult = await doSendAsync();
    }

    return messageResult;
  }
}