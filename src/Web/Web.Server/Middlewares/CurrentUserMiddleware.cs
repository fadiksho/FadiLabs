using Shared.Features.Services;
using Shared.Integration.Extensions;
using System.Diagnostics;

namespace Web.Server.Middlewares;

public class CurrentUserMiddleware(RequestDelegate next)
{
	private readonly RequestDelegate _next = next;

	public async Task InvokeAsync(HttpContext context, ICurrentUser currentUser)
	{
		Debug.WriteLine($"CurrentUserMiddleware User: {context.User.GetIdTokenExpiration() - DateTimeOffset.Now}");
		var user = await currentUser.GetUser();
		Debug.WriteLine($"CurrentUserMiddleware Static User: {user.GetIdTokenExpiration() - DateTimeOffset.Now}");

		await _next(context);
	}
}