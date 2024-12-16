using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Shared.Integration.Extensions;
using Shared.Integration.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Web.Server.Services;

public class ServerASP2 : AuthenticationStateProvider, IHostEnvironmentAuthenticationStateProvider
{
	private readonly PersistentComponentState _state;

	private readonly PersistingComponentStateSubscription _subscription;

	private Task<AuthenticationState> _authenticationStateTask;

	public ServerASP2(PersistentComponentState state)
	{
		_state = state;
		_authenticationStateTask = Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
		AuthenticationStateChanged += OnAuthenticationStateChanged;
		_subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var authState = await _authenticationStateTask;
		Debug.WriteLine($"GetAuthenticationStateAsync: {authState.User.GetIdTokenExpiration() - DateTimeOffset.Now}");
		return authState;
	}
	public void SetAuthenticationState(Task<AuthenticationState> authenticationStateTask)
	{
		_authenticationStateTask = authenticationStateTask ?? throw new ArgumentNullException(nameof(authenticationStateTask));
		NotifyAuthenticationStateChanged(_authenticationStateTask);
	}

	private async void OnAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask)
	{
		var authState = await authenticationStateTask;
		Debug.WriteLine($"ServerASP: {authState.User.GetIdTokenExpiration() - DateTimeOffset.Now}");
		_authenticationStateTask = authenticationStateTask;
	}

	private async Task OnPersistingAsync()
	{
		if (_authenticationStateTask is null)
		{
			throw new UnreachableException($"Authentication state not set in {nameof(RevalidatingServerAuthenticationStateProvider)}.{nameof(OnPersistingAsync)}().");
		}

		var authenticationState = await _authenticationStateTask;
		var principal = authenticationState.User;

		if (principal.Identity?.IsAuthenticated == true)
		{
			var userInfo = UserInfo.BuildUserInfo(principal);
			_state.PersistAsJson(nameof(UserInfo), userInfo);
		}
	}

	protected void Dispose(bool disposing)
	{
		_subscription.Dispose();
		AuthenticationStateChanged -= OnAuthenticationStateChanged;
	}
}