﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.Integration.Extensions;
using Shared.Integration.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Web.Server.Services;

public class ServerASP : RevalidatingServerAuthenticationStateProvider
{
	private readonly IServiceScopeFactory _scopeFactory;
	private readonly PersistentComponentState _state;
	private readonly IdentityOptions _options;

	private readonly PersistingComponentStateSubscription _subscription;

	private Task<AuthenticationState> _authenticationStateTask;

	public ServerASP(
			ILoggerFactory loggerFactory,
			IServiceScopeFactory scopeFactory,
			PersistentComponentState state,
			IOptions<IdentityOptions> options)
			: base(loggerFactory)
	{
		_scopeFactory = scopeFactory;
		_state = state;
		_options = options.Value;
		_authenticationStateTask = Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
		AuthenticationStateChanged += OnAuthenticationStateChanged;
		_subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
	}

	protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(10);

	protected override async Task<bool> ValidateAuthenticationStateAsync(
			AuthenticationState authenticationState, CancellationToken cancellationToken)
	{
		// Get the user manager from a new scope to ensure it fetches fresh data
		await using var scope = _scopeFactory.CreateAsyncScope();
		return ValidateSecurityStampAsync(authenticationState.User);
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var authState = await _authenticationStateTask;
		Debug.WriteLine($"GetAuthenticationStateAsync: {authState.User.GetIdTokenExpiration() - DateTimeOffset.Now}");
		return authState;
	}

	private bool ValidateSecurityStampAsync(ClaimsPrincipal principal)
	{
		if (principal.Identity?.IsAuthenticated is false)
		{
			return false;
		}

		return true;
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

	protected override void Dispose(bool disposing)
	{
		_subscription.Dispose();
		AuthenticationStateChanged -= OnAuthenticationStateChanged;
		base.Dispose(disposing);
	}
}