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

		Claim[] claims = [
				new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
				new Claim(ClaimTypes.Name, userInfo.Name ?? string.Empty),
				new Claim(ClaimTypes.Email, userInfo.Email ?? string.Empty)];

		_authenticationStateTask = Task.FromResult(new AuthenticationState(
			new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: nameof(WasmASP)))));
	}

	public override Task<AuthenticationState> GetAuthenticationStateAsync()
		=> _authenticationStateTask;
}
