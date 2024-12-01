using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Modules.Auth0.Features.Endpoints;
internal static class AuthenticationEndpoints
{
	internal static IEndpointRouteBuilder MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
	{
		var loginLogoutEndpoints = endpoints.MapGroup("/account");

		loginLogoutEndpoints.MapGet("login", async (HttpContext httpContext, string returnUrl = "/") =>
		{
			var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
			.WithRedirectUri(returnUrl)
			.Build();

			await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
		});

		loginLogoutEndpoints.MapGet("logout", async (HttpContext httpContext) =>
		{
			var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
					.WithRedirectUri("/")
					.Build();

			await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
			await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		});

		//loginLogoutEndpoints.MapGet("access-denied", (HttpContent httpContext) =>
		//{
		//	return Results.Redirect("/account/access-denied");
		//});

		return loginLogoutEndpoints;
	}
}
