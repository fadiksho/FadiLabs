using Auth0.OidcClient;
using Microsoft.Extensions.Options;
using Modules.Auth0.Integration.Configuration;
using Shared.Components.Services;
using System.Security.Claims;

namespace Maui.Startup.Services;
public class MauiAuthService : IAuthService
{
	private readonly Auth0Client _auth0Client;
	private readonly MauiUserService _userService;
	private readonly Auth0Configuration _auth0Configuration;
	public MauiAuthService(
		Auth0Client client,
		MauiUserService mauiUserService,
		IOptions<Auth0Configuration> options)
	{
		_auth0Client = client;
		_userService = mauiUserService;
		_auth0Configuration = options.Value;
	}

	public async Task AuthenticateAsync()
	{
		//var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity());
		//string? accessToken = null;
		//string? refreshToken = null;

		var loginResult = await _auth0Client.LoginAsync(new { audience = _auth0Configuration.Audience });

		if (!loginResult.IsError)
		{
			_userService.CurrentUser = loginResult.User;
			_userService.IdToken = loginResult.IdentityToken;
			_userService.AccessToken = loginResult.AccessToken;
			_userService.RefreshToken = loginResult.RefreshToken;

			await SecureStorage.Default.SetAsync("id_token", loginResult.IdentityToken);
			await SecureStorage.Default.SetAsync("access_token", loginResult.AccessToken);
			await SecureStorage.Default.SetAsync("refresh_token", loginResult.RefreshToken);
		}
		else
		{
			SecureStorage.Default.RemoveAll();
		}
	}

	public async Task DeAuthenticateAsync()
	{
		await _auth0Client.LogoutAsync();

		_userService.CurrentUser = new ClaimsPrincipal(new ClaimsIdentity());
		_userService.AccessToken = null;
		_userService.RefreshToken = null;

		SecureStorage.Default.RemoveAll();
	}
}
