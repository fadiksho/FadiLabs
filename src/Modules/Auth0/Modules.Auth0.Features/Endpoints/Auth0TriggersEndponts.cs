using Fadi.Result.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Modules.Auth0.Integration.Models.Auth0Triggers;
using Modules.Shared.Integration.Authorization;
using Modules.User.Integration.User.Commands;
using Modules.User.Integration.User.Queries;
using Shared.Integration.Services;

namespace Modules.Auth0.Features.Endpoints;
internal static class Auth0TriggersEndponts
{
	internal static IEndpointRouteBuilder MapAuth0TriggersEndponts(this IEndpointRouteBuilder endpoints)
	{
		var auth0TriggersEndpoints = endpoints.MapGroup("/api/triggers")
		.RequireAuthorization(LabPolicyNames.ActionTiggerPolicy);

		auth0TriggersEndpoints.MapPost("onExecutePostLogin", async (
			PostLoginActionRequest request,
			[FromServices] IMediator mediator,
			[FromServices] IEnvelopMessageHandler envelopMessage) =>
		{
			var createLabUser = new CreateLabUser
			{
				Auth0UserId = request.Auth0UserId,
				DisplayName = request.DisplayName,
				Email = request.Email,
				EmailVerified = request.EmailVerified,
				ProfilePictureUrl = request.ProfilePictureUrl
			};

			if (!request.IsLabUser)
			{
				var createLabUserResult = await mediator.Send(createLabUser);
				if (createLabUserResult.IsFailed && createLabUserResult.Error is not EntityAlreadyExistsError)
					return Result<GetLabUserRolesResponse>.FromError(createLabUserResult.Error);
			}

			var getUserLabRole = new GetLabUserRoles()
			{
				Auth0UserId = request.Auth0UserId
			};

			var getUserLabRoleResult = await mediator.Send(getUserLabRole);
			if (getUserLabRoleResult.IsSuccess)
			{
				var userPermission = getUserLabRoleResult.Entity.CombinedRolesPermissions;
			}

			return getUserLabRoleResult;
		});

		return endpoints;
	}
}