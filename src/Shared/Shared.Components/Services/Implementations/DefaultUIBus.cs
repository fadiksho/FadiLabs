using Fadi.Result;
using Fadi.Result.Errors;
using MediatR;
using Microsoft.Extensions.Logging;
using Modules.Shared.Integration.Extensions;


namespace Shared.Components.Services.Implementations;

public class DefaultUIBus(IMessageSender messageSender, ILogger<DefaultUIBus> logger) : IUIBus
{
	public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
		where TResponse : notnull, IResult
	{
		TResponse response;
		try
		{
			response = await messageSender.Send(request);
			if (!response.IsSuccess)
			{
				if (response.Error is UnauthentectedError unauthentectedError)
				{

				}
				else if (response.Error is ExceptionError exError)
				{

				}
			}
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred: {ErrorMessage}", ex.Message);

			var exceptionError = new ExceptionError(ex.Message);
			response = request.FromExceptionError(exceptionError);
		}

		return response;
	}
}
