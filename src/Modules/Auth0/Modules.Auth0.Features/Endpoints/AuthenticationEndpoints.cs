using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Modules.Auth0.Features.Endpoints;
internal static class AuthenticationEndpoints
{
	internal static IEndpointRouteBuilder MapAuth0SdkAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
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

		return loginLogoutEndpoints;
	}

	internal static IEndpointConventionBuilder MapOpenIdConnectAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
	{
		var loginLogoutEndpoints = endpoints.MapGroup("/account");

		loginLogoutEndpoints.MapGet("/login", async (HttpContext httpContext, string? returnUrl) =>
		{
			var authenticationProperties = GetAuthProperties(returnUrl);

			await httpContext.ChallengeAsync("Auth0", authenticationProperties);
		});

		// Sign out of the Cookie and OIDC handlers. If you do not sign out with the OIDC handler,
		// the user will automatically be signed back in the next time they visit a page that requires authentication
		// without being able to choose another account.
		loginLogoutEndpoints.MapPost("/logout", ([FromForm] string? returnUrl) => TypedResults.SignOut(GetAuthProperties(returnUrl),
				[CookieAuthenticationDefaults.AuthenticationScheme, "Auth0"]));

		return loginLogoutEndpoints;
	}

	private static AuthenticationProperties GetAuthProperties(string? returnUrl)
	{
		// TODO: Use HttpContext.Request.PathBase instead.
		const string pathBase = "/";

		// Prevent open redirects.
		if (string.IsNullOrEmpty(returnUrl))
		{
			returnUrl = pathBase;
		}
		else if (!Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
		{
			returnUrl = new Uri(returnUrl, UriKind.Absolute).PathAndQuery;
		}
		else if (returnUrl[0] != '/')
		{
			returnUrl = $"{pathBase}{returnUrl}";
		}

		return new AuthenticationProperties { RedirectUri = returnUrl };
	}
}

