using Fadi.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.Features.Server.Behaviours;
public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull, IRequest<TResponse>
	where TResponse : IResult
{
	private readonly ILogger _logger;

	public LoggingBehaviour(
		ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
	{
		_logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;
		_logger.LogInformation("Request: {Name}.", requestName);

		var response = await next();

		var responseName = typeof(TResponse).Name;
		_logger.LogInformation("Response: {Name}.\n", responseName);

		return response;
	}
}
