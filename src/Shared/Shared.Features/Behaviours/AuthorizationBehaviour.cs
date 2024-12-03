using Fadi.Result.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Modules.Authorization.Integration.Authorization;
using Modules.Shared.Integration.Authorization;
using Modules.Shared.Integration.Extensions;
using Shared.Features.Services;
using System.Reflection;

namespace Shared.Features.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse>(
	ICurrentUser currentUser,
	IAuthorizationService authorizationService) : IPipelineBehavior<TRequest, TResponse>
		where TRequest : notnull, IRequest<TResponse>
		where TResponse : Fadi.Result.IResult
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var authorizeAttributes = request.GetType().GetCustomAttributes<LabAuthorizeAttribute>();

		if (!authorizeAttributes.Any())
		{
			return await next();
		}

		var user = currentUser.GetUser();

		foreach (var authorizeAttribute in authorizeAttributes)
		{
			var requiredPermission = authorizeAttribute.Permissions;
			var authorizationResult = await authorizationService.AuthorizeAsync(user, requiredPermission);
			if (!authorizationResult.Succeeded)
			{
				return next.FromError(request, new UnauthorizedError());
			}
		}

		return await next();
	}
}