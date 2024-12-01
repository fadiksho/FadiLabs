using Fadi.Result;
using MediatR;

namespace Modules.Shared.Integration.Extensions;

public static class ResultErrorExtensions
{
	public static TResponse FromExceptionError<TResponse>(this IRequest<TResponse> request, IResultError error)
	{
		var errorResult = typeof(TResponse)
			.GetMethod(nameof(Result.FromError))
			?.MakeGenericMethod(typeof(IResultError))
			.Invoke(request, [error]);

		return errorResult == null
			? throw new ApplicationException()
			: (TResponse)errorResult;
	}

	public static TReturn FromError<TReturn, TRequest>(this RequestHandlerDelegate<TReturn> requestHandler, TRequest request, IResultError error)
	{
		var errorResult = typeof(TReturn)
			.GetMethod(nameof(Result.FromError))
			?.MakeGenericMethod(typeof(IResultError))
			.Invoke(request, [error]);

		return errorResult == null
			? throw new ApplicationException()
			: (TReturn)errorResult;
	}
}
