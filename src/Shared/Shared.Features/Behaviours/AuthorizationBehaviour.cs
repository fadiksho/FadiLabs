using Fadi.Result.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Modules.Shared.Integration.Authorization;
using Modules.Shared.Integration.Extensions;
using Shared.Features.Services;
using Shared.Integration.Extensions;
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
		IEnumerable<AuthorizeAttribute> authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

		if (!authorizeAttributes.Any())
		{
			return await next();
		}

		var user = currentUser.GetUser();

		if (user == null || !user.IsAuthenticated())
		{
			return next.FromError(request, new UnauthentectedError());
		}

		foreach (var authorizeAttribute in authorizeAttributes)
		{
			var authorizationResult = AuthorizationResult.Success();

			if (authorizeAttribute is LabAuthorizeAttribute labAuthorizeAttribute)
			{
				authorizationResult = await authorizationService.AuthorizeAsync(user, labAuthorizeAttribute.LabsPermissions);
			}

			else if (!string.IsNullOrEmpty(authorizeAttribute.Policy))
			{
				authorizationResult = await authorizationService.AuthorizeAsync(user, authorizeAttribute.Policy);
			}

			if (authorizationResult.Succeeded == false)
			{
				return next.FromError(request, new UnauthorizedError());
			}
		}

		return await next();
	}
}