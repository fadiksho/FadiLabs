global using Fadi.Result;
global using MediatR;
global using Modules.Auth0.Integration.Models;
global using Modules.Shared.Integration.Models;
global using System.Text.Json;
using Auth0.AspNetCore.Authentication;
using Auth0Net.DependencyInjection;
using Fadi.Result.Errors;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Net.Http.Headers;
using Modules.Auth0.Components;
using Modules.Auth0.Features.Endpoints;
using Modules.Auth0.Features.Services.Implementaions;
using Modules.Auth0.Features.Utils;
using Modules.Auth0.Integration.Configuration;
using Modules.Shared.Integration.Authorization;
using Shared.Features.Configuration;
using Shared.Integration;
using System.Security.Claims;

namespace Modules.Auth0.Features;

public static class Program
{
	public static IServiceCollection AddAuth0ModuleServices(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
	{
		services.AddAuth0ModuleComponentsServices(config);

		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
		});

		services.Configure<Auth0Configuration>(config.GetSection(Auth0Configuration.SectionName));

		var _auth0Options = config.GetSection(Auth0Configuration.SectionName)
			.Get<Auth0Configuration>() ?? new();

		var _devTunnelOptions = config.GetSection(DevTunnelConfiguration.SectionName)
			.Get<DevTunnelConfiguration>() ?? new();

		JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
		var authBuilder = services.AddAuthentication(options =>
		{
			options.DefaultScheme = "JWT_OR_COOKIE";
			options.DefaultChallengeScheme = "JWT_OR_COOKIE";
			options.DefaultSignInScheme = "JWT_OR_COOKIE";
		});

		//authBuilder.AddOpenIdConnectWithAuth0Sdk(services, config);
		authBuilder.AddOpenIdConnectWithoutAuth0Sdk(services, config);

		authBuilder.AddJwtBearer(options =>
		{
			options.Authority = $"https://{_auth0Options.Domain}/";
			options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
			{
				RoleClaimType = SharedConstents.LabsClaimTypes.Role,
				NameClaimType = SharedConstents.LabsClaimTypes.Name,
				ValidAudience = _auth0Options.Audience,
				ValidIssuer = _auth0Options.Domain,
			};
			options.Events = new JwtBearerEvents
			{
				OnChallenge = async context =>
				{
					// Prevent the default behavior of the JwtBearer middleware
					context.HandleResponse();

					await AuthenticationHelper
						.WriteJsonResponse(context.HttpContext, StatusCodes.Status401Unauthorized, new UnauthentectedError());
				},
				OnForbidden = async context =>
				{
					await AuthenticationHelper
						.WriteJsonResponse(context.HttpContext, StatusCodes.Status403Forbidden, new UnauthorizedError());
				}
			};
		});

		authBuilder.AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
		{
			options.ForwardDefaultSelector = context =>
			{
				var authorization = context.Request.Headers[HeaderNames.Authorization].ToString();

				if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer"))
				{
					return JwtBearerDefaults.AuthenticationScheme;
				}

				return CookieAuthenticationDefaults.AuthenticationScheme;
			};
		});

		services.AddAuthorization(options =>
		{
			var defaultAuthorizationPolicy = new AuthorizationPolicyBuilder(
				"JWT_OR_COOKIE")
			.RequireAuthenticatedUser()
			.Build();

			var auth0ActionTriggerPolicy = new AuthorizationPolicyBuilder(
				JwtBearerDefaults.AuthenticationScheme)
				.RequireAuthenticatedUser()
				.RequireClaim("scope", "trigger-actions")
				.Build();

			options.AddPolicy(LabPolicyNames.ActionTiggerPolicy, auth0ActionTriggerPolicy);

			options.DefaultPolicy = defaultAuthorizationPolicy;
		});

		services.AddAuth0AuthenticationClient(config =>
		{
			config.Domain = _auth0Options.Domain;
			config.ClientId = _auth0Options.ClientId;
			config.ClientSecret = _auth0Options.ClientSecret;
		});
		services.AddAuth0ManagementClient()
			.AddManagementAccessToken();

		return services;
	}

	public static RazorComponentsEndpointConventionBuilder MapAuth0ModulePages(this RazorComponentsEndpointConventionBuilder builder)
	{
		return builder.AddAdditionalAssemblies(
			typeof(Components.Program).Assembly);
	}

	public static void MapAuth0ModleEndPoints(this IEndpointRouteBuilder endpoints)
	{
		//endpoints.MapAuth0SdkAuthenticationEndpoints();
		endpoints.MapOpenIdConnectAuthenticationEndpoints();
		endpoints.MapAuth0TriggersEndponts();
	}

	private static void AddOpenIdConnectWithAuth0Sdk(this AuthenticationBuilder builder, IServiceCollection services, IConfiguration config)
	{
		var _auth0Options = config.GetSection(Auth0Configuration.SectionName)
			.Get<Auth0Configuration>() ?? new();

		var _devTunnelOptions = config.GetSection(DevTunnelConfiguration.SectionName)
			.Get<DevTunnelConfiguration>() ?? new();

		builder.AddAuth0WebAppAuthentication(options =>
		{
			options.Domain = _auth0Options.Domain;
			options.ClientId = _auth0Options.ClientId;
			options.Scope = "openid profile email";
			options.CallbackPath = new PathString("/callback");
			options.AccessDeniedPath = new PathString("/account/access-denied");
			options.OpenIdConnectEvents = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
			{
				OnRedirectToIdentityProvider = context =>
				{
					if (_devTunnelOptions.IsEnabled && !string.IsNullOrEmpty(_devTunnelOptions.Url))
					{
						var testo = context.ProtocolMessage.PostLogoutRedirectUri;
						context.ProtocolMessage.RedirectUri = $"{_devTunnelOptions.Url}{options.CallbackPath}";
					}

					return Task.CompletedTask;
				},
				OnRedirectToIdentityProviderForSignOut = context =>
				{
					var testo = context.ProtocolMessage.PostLogoutRedirectUri;
					return Task.CompletedTask;
				},
				OnTokenValidated = context =>
				{
					if (context.Principal?.Identity is not ClaimsIdentity identity)
					{
						return Task.CompletedTask;
					}

					context.Principal = context.Principal
						.UpdateClaimTypes(SharedConstents.LabsClaimTypes.Name, SharedConstents.LabsClaimTypes.Role);

					return Task.CompletedTask;
				}
			};
		});

		services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
		{
			options.AccessDeniedPath = "/account/access-denied";
			options.LogoutPath = "/account/logout";
			options.LoginPath = "/account/login";
			options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
			options.Events.OnRedirectToLogin = async context =>
			{
				if (context.Request.Path.StartsWithSegments("/api")
								|| IsAjaxRequest(context.Request))
				{
					await AuthenticationHelper
						.WriteJsonResponse(context.HttpContext, StatusCodes.Status401Unauthorized, new UnauthentectedError());
				}
				else
					context.Response.Redirect(context.RedirectUri);
			};

			options.Events.OnRedirectToAccessDenied = async context =>
			{
				if (context.Request.Path.StartsWithSegments("/api")
								|| IsAjaxRequest(context.Request))
				{
					await AuthenticationHelper
						.WriteJsonResponse(context.HttpContext, StatusCodes.Status403Forbidden, new UnauthorizedError());
				}
				else
					context.Response.Redirect(context.RedirectUri);
			};
		});
	}

	private static void AddOpenIdConnectWithoutAuth0Sdk(this AuthenticationBuilder builder, IServiceCollection services, IConfiguration config)
	{
		var _auth0Options = config.GetSection(Auth0Configuration.SectionName)
			.Get<Auth0Configuration>() ?? new();

		var _devTunnelOptions = config.GetSection(DevTunnelConfiguration.SectionName)
			.Get<DevTunnelConfiguration>() ?? new();

		builder.AddOpenIdConnect("Auth0", oidcOptions =>
		{
			oidcOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			// ........................................................................

			// ........................................................................
			// The "openid" and "profile" scopes are required for the OIDC handler 
			// and included by default.
			oidcOptions.Scope.Clear();
			oidcOptions.Scope.Add(OpenIdConnectScope.OpenIdProfile);
			oidcOptions.Scope.Add(OpenIdConnectScope.Email);
			// ........................................................................

			// ........................................................................
			// The following paths must match the redirect and post logout redirect 
			// paths configured when registering the application with the OIDC provider. 
			oidcOptions.CallbackPath = new PathString("/callback"); ;
			//oidcOptions.SignedOutCallbackPath = new PathString("/signout-callback-oidc");
			// ........................................................................

			// ........................................................................
			// The RemoteSignOutPath is the "Front-channel logout URL" for remote single 
			// sign-out. The default value is "/signout-oidc".
			//oidcOptions.RemoteSignOutPath = new PathString("/signout-oidc");
			// ........................................................................

			// ........................................................................
			oidcOptions.Authority = $"https://{_auth0Options.Domain}";
			// ........................................................................

			// ........................................................................
			// Set the Client ID for the app. Set the {CLIENT ID} placeholder to
			// the Client ID.
			oidcOptions.ClientId = _auth0Options.ClientId;
			// ........................................................................

			// ........................................................................
			// Setting ResponseType to "code" configures the OIDC handler to use 
			// authorization code flow. Implicit grants and hybrid flows are unnecessary
			// in this mode.
			oidcOptions.ResponseType = OpenIdConnectResponseType.IdToken;
			// ........................................................................

			// ........................................................................
			// Set MapInboundClaims to "false" to obtain the original claim types from 
			// the token. Many OIDC servers use "name" and "role"/"roles" rather than 
			// the SOAP/WS-Fed defaults in ClaimTypes. Adjust these values if your 
			// identity provider uses different claim types.

			oidcOptions.MapInboundClaims = false;
			oidcOptions.TokenValidationParameters.NameClaimType = SharedConstents.LabsClaimTypes.Name;
			oidcOptions.TokenValidationParameters.RoleClaimType = SharedConstents.LabsClaimTypes.Role;
			// ........................................................................

			// ........................................................................
			// Many OIDC providers work with the default issuer validator, but the
			// configuration must account for the issuer parameterized with "{TENANT ID}" 
			// returned by the "common" endpoint's /.well-known/openid-configuration
			// For more information, see
			// https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1731

			//var microsoftIssuerValidator = AadIssuerValidator.GetAadIssuerValidator(oidcOptions.Authority);
			//oidcOptions.TokenValidationParameters.IssuerValidator = microsoftIssuerValidator.Validate;
			// ........................................................................

			// ........................................................................
			// OIDC connect options set later via ConfigureCookieOidcRefresh
			//
			// (1) The "offline_access" scope is required for the refresh token.
			//
			// (2) SaveTokens is set to true, which saves the access and refresh tokens
			// in the cookie, so the app can authenticate requests for weather data and
			// use the refresh token to obtain a new access token on access token
			// expiration.
			// ........................................................................
			oidcOptions.AccessDeniedPath = new PathString("/account/access-denied");
			oidcOptions.Events.OnRedirectToIdentityProvider = context =>
			{
				if (_devTunnelOptions.IsEnabled && !string.IsNullOrEmpty(_devTunnelOptions.Url))
				{
					context.ProtocolMessage.RedirectUri = $"{_devTunnelOptions.Url}{oidcOptions.CallbackPath}";
				}

				return Task.CompletedTask;
			};
		})
			.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

		services.AddSingleton<CookieOidcRefresher>();

		services
			.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
			.Configure<CookieOidcRefresher>((cookieOptions, refresher) =>
			{
				cookieOptions.Events.OnValidatePrincipal = context => refresher.ValidateOrRefreshCookieAsync(context, "Auth0");
			});

		services.AddOptions<OpenIdConnectOptions>("Auth0").Configure(oidcOptions =>
		{
			// Request a refresh_token.
			oidcOptions.Scope.Add(OpenIdConnectScope.OfflineAccess);
			// Store the refresh_token.
			oidcOptions.SaveTokens = true;
		});
	}

	private static bool IsAjaxRequest(HttpRequest request)
	{
		return string.Equals(request.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) ||
				string.Equals(request.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.Ordinal);
	}
}
