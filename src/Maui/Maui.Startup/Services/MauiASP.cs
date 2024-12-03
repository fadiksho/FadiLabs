using Microsoft.AspNetCore.Components.Authorization;

namespace Maui.Startup.Services;

public class MauiASP : AuthenticationStateProvider
{
	private AuthenticationState _currentUser;

	private readonly MauiUserService _authService;
	public MauiASP(MauiUserService mauiUserService)
	{
		_authService = mauiUserService;
		_currentUser = new AuthenticationState(_authService.CurrentUser);

		_authService.UserChanged += (newUser) =>
		{
			_currentUser = new AuthenticationState(newUser);
			NotifyAuthenticationStateChanged(Task.FromResult(_currentUser));
		};
	}

	public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
				Task.FromResult(_currentUser);
}
