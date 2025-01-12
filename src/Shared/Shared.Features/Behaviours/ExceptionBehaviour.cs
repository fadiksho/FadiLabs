using Fadi.Result.Errors;
using MediatR;
using Microsoft.Extensions.Logging;
using Modules.Shared.Integration.Extensions;
using Shared.Features.Server.Exceptions;

namespace Shared.Features.Server.Behaviours;

public class ExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull, IRequest<TResponse>
	where TResponse : Fadi.Result.IResult
{
	private readonly ILogger _logger;

	public ExceptionBehaviour(ILogger<ExceptionBehaviour<TRequest, TResponse>> logger)
	{
		_logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		try
		{
			return await next();
		}
		catch (UnauthentectedException)
		{
			return next.FromError(request, new UnauthentectedError());
		}
		catch (Exception ex)
		{
			var requestName = typeof(TRequest).Name;

			_logger.LogError(ex, "Unhandled Exception for Request {Name} {@Request}", requestName, request);

			return next.FromError(request, new ExceptionError(ex.Message));
		}
	}
}
