using Fadi.Result.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Modules.Shared.Integration.Authorization;
using Modules.User.Integration;
using Shared.Integration.Services;

namespace Modules.Auth0.Features.Endpoints;
internal static class Auth0TriggersEndponts
{
	internal static IEndpointRouteBuilder MapAuth0TriggersEndponts(this IEndpointRouteBuilder endpoints)
	{
		var auth0TriggersEndpoints = endpoints.MapGroup("/api/triggers")
			.RequireAuthorization(LabPolicyNames.ActionTiggerPolicy);

		auth0TriggersEndpoints.MapPost("onExecutePostLogin", async (
			CreateLabUser newUser,
			[FromServices] IMediator mediator,
			[FromServices] IEnvelopMessageHandler envelopMessage,
			ILoggerFactory loggerFactory) =>
		{
			var logger = loggerFactory.CreateLogger(nameof(Auth0TriggersEndponts));
			var result = await mediator.Send(newUser);

			if (result.IsSuccess)
			{
				// User Created!
				logger.LogInformation("User Created!");
			}
			else if (result.Error is EntityAlreadyExistsError)
			{
				// User Already has account in labdb
				logger.LogInformation("User Already has account in labdb");
			}
			else
			{
				// not implemented yet
				logger.LogInformation("Not implemented yet!");
			}

			return result;
		});

		return endpoints;
	}
}