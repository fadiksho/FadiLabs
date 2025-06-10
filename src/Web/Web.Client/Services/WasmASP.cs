using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Integration.Models;
using System.Security.Claims;

namespace Web.Client.Services;

public class WasmASP : AuthenticationStateProvider
{
	private static readonly Task<AuthenticationState> _defaultUnauthenticatedTask =
				Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

	private readonly Task<AuthenticationState> _authenticationStateTask = _defaultUnauthenticatedTask;

	public WasmASP(PersistentComponentState state)
	{
		if (!state.TryTakeFromJson<UserInfo>(nameof(UserInfo), out var userInfo) || userInfo is null)
		{
			return;
		}

		_authenticationStateTask = Task.FromResult(new AuthenticationState(UserInfo.BuildClaimsPrincipal(userInfo)));
	}

	public override Task<AuthenticationState> GetAuthenticationStateAsync()
		=> _authenticationStateTask;
}
