using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Modules.Auth0.Integration.Configuration;
using Shared.Features.Configuration;

namespace Modules.Auth0.Features.Services;

internal class CustomOpenIdConnectEvents
	(IOptions<DevTunnelConfiguration> devTunnelOptions,
	IOptions<OpenIdConnectOptions> oidcOptions,
	IOptions<Auth0Configuration> auth0Options) : OpenIdConnectEvents
{
	private readonly DevTunnelConfiguration _devTunnelConfig = devTunnelOptions.Value;
	private readonly OpenIdConnectOptions _oidcConfig = oidcOptions.Value;
	private readonly Auth0Configuration _authConfig = auth0Options.Value;

	public override Task RedirectToIdentityProvider(RedirectContext context)
	{
		context.ProtocolMessage.SetParameter("audience", _authConfig.Audience);

		if (_devTunnelConfig.IsEnabled && !string.IsNullOrEmpty(_devTunnelConfig.Url))
		{
			context.ProtocolMessage.RedirectUri = $"{_devTunnelConfig.Url}{_oidcConfig.CallbackPath}";
		}

		return Task.CompletedTask;
	}

	public override Task AccessDenied(AccessDeniedContext context)
	{
		context.Request.Form.TryGetValue("error_description", out var errorMessage);

		return Task.CompletedTask;
	}

	public override Task TokenResponseReceived(TokenResponseReceivedContext context)
	{
		return Task.CompletedTask;
	}

	public override Task TokenValidated(TokenValidatedContext context)
	{
		var tokens = new List<AuthenticationToken>();

		//if (!string.IsNullOrEmpty(context.TokenEndpointResponse.IdToken))
		//{
		//	// Decode the id_token to get the expiration time
		//	var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
		//	var jwtToken = handler.ReadJwtToken(context.TokenEndpointResponse.IdToken);

		//	// Extract the exp claim (Unix timestamp)
		//	var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

		//	if (long.TryParse(expClaim, out var expUnix))
		//	{
		//		// Convert to DateTimeOffset
		//		var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expUnix);

		//		// Store expiration time in AuthenticationProperties
		//		tokens.Add(new AuthenticationToken
		//		{
		//			Name = "id_token_expiration",
		//			Value = expirationTime.ToString("o")
		//		});
		//	}
		//}

		//if (!string.IsNullOrEmpty(context.TokenEndpointResponse?.IdToken))
		//{
		//	tokens.Add(new AuthenticationToken
		//	{
		//		Name = OpenIdConnectParameterNames.IdToken,
		//		Value = context.TokenEndpointResponse.IdToken,
		//	});
		//}

		if (!string.IsNullOrEmpty(context.TokenEndpointResponse?.RefreshToken))
		{
			tokens.Add(new AuthenticationToken
			{
				Name = OpenIdConnectParameterNames.RefreshToken,
				Value = context.TokenEndpointResponse.RefreshToken
			});
		}

		context.Properties?.StoreTokens(tokens);

		return base.TokenValidated(context);
	}

	public override Task MessageReceived(MessageReceivedContext context)
	{
		return base.MessageReceived(context);
	}
}
