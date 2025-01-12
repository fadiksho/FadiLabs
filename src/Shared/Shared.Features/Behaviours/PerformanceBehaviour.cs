using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shared.Features.Server.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull, IRequest<TResponse>
	where TResponse : Fadi.Result.IResult
{
	private readonly ILogger _logger;
	private readonly Stopwatch _timer;

	public PerformanceBehaviour(
		ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
	{
		_logger = logger;
		_timer = new Stopwatch();
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;
		_logger.LogInformation("Running Request: {Name}.", requestName);

		_timer.Start();
		var response = await next();
		_timer.Stop();

		_logger.LogInformation("Endding Request: {Name} ({ElapsedMilliseconds} milliseconds).\n",
			requestName, _timer.ElapsedMilliseconds);

		//if (_timer.ElapsedMilliseconds > 100)
		//{

		//	_logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds)",
		//			requestName, _timer.ElapsedMilliseconds);
		//}

		return response;
	}
}