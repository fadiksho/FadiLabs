using Auth0.OidcClient;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Results;
using Microsoft.Extensions.Options;
using Modules.Auth0.Integration.Configuration;
using System.Security.Claims;

namespace Maui.Startup.Services;
public class MauiUserService
{
	private readonly Auth0Configuration _auth0Config;
	private readonly Auth0Client _auth0Client;

	public MauiUserService(
		Auth0Client auth0Client,
		IOptions<Auth0Configuration> options)
	{
		_auth0Config = options.Value;
		_auth0Client = auth0Client;
	}

	public event Action<ClaimsPrincipal>? UserChanged;
	private ClaimsPrincipal? _currentUser;

	public ClaimsPrincipal CurrentUser
	{
		get { return _currentUser ?? new(); }
		set
		{
			_currentUser = value;

			if (UserChanged is not null)
			{
				UserChanged(_currentUser);
			}
		}
	}

	public string? AccessToken { get; set; }
	public string? RefreshToken { get; set; }

	public async Task LoadUserAuthenticationAsync()
	{
		var idToken = await SecureStorage.Default.GetAsync("id_token");

		if (idToken != null)
		{
			var doc = await new HttpClient()
				.GetDiscoveryDocumentAsync($"https://{_auth0Config.Domain}");
			var validator = new JwtHandlerIdentityTokenValidator();
			var options = new OidcClientOptions
			{
				ClientId = _auth0Config.ClientId,
				ProviderInformation = new ProviderInformation
				{
					IssuerName = doc.Issuer,
					KeySet = doc.KeySet
				}
			};

			var validationResult = await validator.ValidateAsync(idToken, options);

			if (!validationResult.IsError)
			{
				CurrentUser = validationResult.User;
				AccessToken = await SecureStorage.Default.GetAsync("access_token");
				RefreshToken = await SecureStorage.Default.GetAsync("refresh_token");
			}
		}
	}
	public async Task<RefreshTokenResult> RefreshTokenAsync()
	{
		// ToDo: Check exception try/catch
		var refreshResult = await _auth0Client.RefreshTokenAsync(RefreshToken);

		SecureStorage.Default.Remove("access_token");
		SecureStorage.Default.Remove("refresh_token");
		SecureStorage.Default.Remove("id_token");

		if (!refreshResult.IsError)
		{
			AccessToken = refreshResult.AccessToken;
			RefreshToken = refreshResult.RefreshToken;

			await SecureStorage.Default.SetAsync("access_token", AccessToken);
			await SecureStorage.Default.SetAsync("id_token", refreshResult.IdentityToken);

			if (RefreshToken != null)
			{
				await SecureStorage.Default.SetAsync("refresh_token", RefreshToken);
			}
		}

		return refreshResult;
	}
}
