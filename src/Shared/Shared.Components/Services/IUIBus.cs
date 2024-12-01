using Fadi.Result;
using MediatR;

namespace Shared.Components.Services;

public interface IUIBus
{
  Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    where TResponse : notnull, IResult;
}
