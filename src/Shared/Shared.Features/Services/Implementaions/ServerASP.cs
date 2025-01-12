using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Shared.Integration.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Shared.Features.Server.Services.Implementaions;

public sealed class ServerASP : AuthenticationStateProvider, IHostEnvironmentAuthenticationStateProvider, IDisposable
{
	private readonly PersistentComponentState _persistentComponentState;
	private readonly PersistingComponentStateSubscription _persistingComponentStateSubscription;

	private Task<AuthenticationState> _authenticationStateTask;

	public ServerASP(PersistentComponentState persistentComponentState)
	{
		_persistentComponentState = persistentComponentState;
		_persistingComponentStateSubscription = _persistentComponentState.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);

		_authenticationStateTask = Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
		AuthenticationStateChanged += OnAuthenticationStateChanged;
	}

	public override Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		return _authenticationStateTask;
	}

	public void SetAuthenticationState(Task<AuthenticationState> authenticationStateTask)
	{
		_authenticationStateTask = authenticationStateTask;
		NotifyAuthenticationStateChanged(authenticationStateTask);
	}

	private void OnAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask)
	{
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
			_persistentComponentState.PersistAsJson(nameof(UserInfo), userInfo);
		}
	}

	public void Dispose()
	{
		_persistingComponentStateSubscription.Dispose();
		AuthenticationStateChanged -= OnAuthenticationStateChanged;
	}
}