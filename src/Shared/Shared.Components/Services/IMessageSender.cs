using Fadi.Result;
using MediatR;

namespace Shared.Components.Services;

public interface IMessageSender
{
  Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    where TResponse : notnull, IResult;
}
