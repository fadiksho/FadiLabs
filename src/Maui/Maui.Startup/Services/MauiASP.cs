using Microsoft.AspNetCore.Components.Authorization;

namespace Maui.Startup.Services;

public class MauiASP : AuthenticationStateProvider
{
	private AuthenticationState currentUser;

	private readonly MauiUserService authService;
	public MauiASP(MauiUserService mauiUserService)
	{
		authService = mauiUserService;
		currentUser = new AuthenticationState(authService.CurrentUser);

		authService.UserChanged += (newUser) =>
		{
			currentUser = new AuthenticationState(newUser);
			NotifyAuthenticationStateChanged(Task.FromResult(currentUser));
		};
	}

	public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
				Task.FromResult(currentUser);
}
