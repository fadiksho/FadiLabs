global using Fadi.Result;
global using MediatR;
global using Modules.Auth0.Integration.Models;
global using Modules.Shared.Integration.Models;
global using System.Text.Json;
using Auth0.AspNetCore.Authentication;
using Auth0Net.DependencyInjection;
using Fadi.Result.Errors;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;
using Modules.Auth0.Components;
using Modules.Auth0.Features.Configuration;
using Modules.Auth0.Features.Endpoints;
using Modules.Auth0.Features.Utils;
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

		services
			.Configure<Auth0Configuration>(config.GetSection(Auth0Configuration.SectionName));

		JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
		var authBuilder = services.AddAuthentication(options =>
		{
			options.DefaultScheme = "JWT_OR_COOKIE";
			options.DefaultChallengeScheme = "JWT_OR_COOKIE";
			options.DefaultSignInScheme = "JWT_OR_COOKIE";
		});

		var _auth0Options = config.GetSection(Auth0Configuration.SectionName)
			.Get<Auth0Configuration>() ?? new();

		var _devTunnelOptions = config.GetSection(DevTunnelConfiguration.SectionName)
			.Get<DevTunnelConfiguration>() ?? new();

		authBuilder.AddAuth0WebAppAuthentication(options =>
		{
			options.Domain = _auth0Options.Domain;
			options.ClientId = _auth0Options.ClientId;
			options.Scope = "openid profile email";
			options.CallbackPath = new PathString("/callback");
			options.OpenIdConnectEvents = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
			{
				OnRedirectToIdentityProvider = context =>
				{
					if (_devTunnelOptions.IsEnabled && !string.IsNullOrEmpty(_devTunnelOptions.Url))
					{
						context.ProtocolMessage.RedirectUri = $"{_devTunnelOptions.Url}{options.CallbackPath}";
					}

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
				},
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
		endpoints.MapAuthenticationEndpoints();
		endpoints.MapAuth0TriggersEndponts();
	}

	private static bool IsAjaxRequest(HttpRequest request)
	{
		return string.Equals(request.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) ||
				string.Equals(request.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.Ordinal);
	}
}
