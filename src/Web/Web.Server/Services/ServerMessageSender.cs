using MediatR;
using Shared.Components.Services;

namespace Web.Server.Services;

internal class ServerMessageSender(IMediator mediator) : IMessageSender
{
  public Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    where TResponse : notnull, Fadi.Result.IResult
  {
    return mediator.Send(request);
  }
}
