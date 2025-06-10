using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Shared.Features.Services;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Modules.Auth0.Features.Services;

internal class CustomCookieAuthenticationEvents
	(IOptionsMonitor<OpenIdConnectOptions> oidcOptionsMonitor,
	CircuitServicesAccessor circuitServicesAccessor, AuthenticationStateProvider authenticationStateProvider) : CookieAuthenticationEvents
{
	private readonly OpenIdConnectOptions _oidcOptions = oidcOptionsMonitor.Get("Auth0");
	private readonly OpenIdConnectProtocolValidator _oidcTokenValidator = new()
	{
		// We no longer have the original nonce cookie which is deleted at the end of the authorization code flow having served its purpose.
		// Even if we had the nonce, it's likely expired. It's not intended for refresh requests. Otherwise, we'd use oidcOptions.ProtocolValidator.
		RequireNonce = false,
	};
	private readonly CircuitServicesAccessor _circuitServicesAccessor = circuitServicesAccessor;
	private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

	public override async Task ValidatePrincipal(CookieValidatePrincipalContext validateContext)
	{
		var idTokenExpirationText = validateContext.Principal?.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
		if (!long.TryParse(idTokenExpirationText, out long expUnixTime))
		{
			return;
		}

		var idTokenExpiration = DateTimeOffset.FromUnixTimeSeconds(expUnixTime);
		var now = _oidcOptions.TimeProvider!.GetUtcNow();

		var isTokenExpired = now > idTokenExpiration;
		Debug.WriteLine($"Token expires in: {idTokenExpiration - now}");
		if (!isTokenExpired)
		{
			return;
		}

		Debug.WriteLine($"Token Expired at: {idTokenExpiration:dd/MM/yyyy HH:mm:ss}.");

		var oidcConfiguration = await _oidcOptions.ConfigurationManager!.GetConfigurationAsync(validateContext.HttpContext.RequestAborted);
		var tokenEndpoint = oidcConfiguration.TokenEndpoint
				?? throw new InvalidOperationException("Cannot refresh id_token. TokenEndpoint missing!");

		// Use the refresh token to obtain a new id_token
		var refreshToken = validateContext.Properties.GetTokenValue("refresh_token");
		if (string.IsNullOrEmpty(refreshToken))
		{
			validateContext.RejectPrincipal(); // If no refresh token is present, reject the principal
			return;
		}

		using var refreshResponse = await _oidcOptions.Backchannel.PostAsync(tokenEndpoint,
				new FormUrlEncodedContent(new Dictionary<string, string?>()
				{
					["grant_type"] = "refresh_token",
					["client_id"] = _oidcOptions.ClientId,
					["client_secret"] = _oidcOptions.ClientSecret,
					["scope"] = string.Join(" ", _oidcOptions.Scope),
					["refresh_token"] = refreshToken,
				}));

		if (!refreshResponse.IsSuccessStatusCode)
		{
			validateContext.RejectPrincipal(); // Reject if the refresh request fails
			return;
		}

		var refreshJson = await refreshResponse.Content.ReadAsStringAsync();
		var message = new OpenIdConnectMessage(refreshJson);

		// Validate the new id_token
		var validationParameters = _oidcOptions.TokenValidationParameters.Clone();
		if (_oidcOptions.ConfigurationManager is BaseConfigurationManager baseConfigurationManager)
		{
			validationParameters.ConfigurationManager = baseConfigurationManager;
		}
		else
		{
			validationParameters.ValidIssuer = oidcConfiguration.Issuer;
			validationParameters.IssuerSigningKeys = oidcConfiguration.SigningKeys;
		}

		var validationResult = await _oidcOptions.TokenHandler.ValidateTokenAsync(message.IdToken, validationParameters);

		if (!validationResult.IsValid)
		{
			validateContext.RejectPrincipal();
			return;
		}

		var validatedIdToken = JwtSecurityTokenConverter.Convert(validationResult.SecurityToken as JsonWebToken);
		validatedIdToken.Payload["nonce"] = null;
		_oidcTokenValidator.ValidateTokenResponse(new()
		{
			ProtocolMessage = message,
			ClientId = _oidcOptions.ClientId,
			ValidatedIdToken = validatedIdToken,
		});

		// Replace the principal and update the tokens
		validateContext.ShouldRenew = true;
		var updatedUser = new ClaimsPrincipal(validationResult.ClaimsIdentity);
		validateContext.ReplacePrincipal(updatedUser);

		var authState = _circuitServicesAccessor.Services?.GetService<AuthenticationStateProvider>();
		if (authState != null && authState is IHostEnvironmentAuthenticationStateProvider authHostState)
		{
			authHostState.SetAuthenticationState(Task.FromResult(new AuthenticationState(updatedUser)));
		}

		if (_authenticationStateProvider is IHostEnvironmentAuthenticationStateProvider authHostState2)
		{
			authHostState2.SetAuthenticationState(Task.FromResult(new AuthenticationState(updatedUser)));
		}

		var expClaim = validationResult.ClaimsIdentity.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;

		if (long.TryParse(expClaim, out var expUnix))
		{
			// Convert to DateTimeOffset
			idTokenExpiration = DateTimeOffset.FromUnixTimeSeconds(expUnix);
		}

		validateContext.Properties.StoreTokens(
		[
				//new() { Name = "id_token", Value = message.IdToken },
				new() { Name = "refresh_token", Value = message.RefreshToken },
				//new() { Name = "id_token_expiration", Value = idTokenExpiration.ToString("o", CultureInfo.InvariantCulture) }
		]);
	}
}
