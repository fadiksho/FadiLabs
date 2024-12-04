using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Modules.Auth0.Features.Endpoints;
internal static class Auth0TriggersEndponts
{
	internal static IEndpointRouteBuilder MapAuth0TriggersEndponts(this IEndpointRouteBuilder endpoints)
	{
		var auth0TriggersEndpoints = endpoints.MapGroup("/api/triggers")
			.RequireAuthorization(Auth0LabConstents.Policies.ActionTiggerPolicy);

		auth0TriggersEndpoints.MapPost("onExecutePostLogin", () =>
		{

		});

		return endpoints;
	}
}
