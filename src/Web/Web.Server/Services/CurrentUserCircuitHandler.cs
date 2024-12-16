using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Shared.Features.Services;
using Shared.Integration.Extensions;
using System.Diagnostics;

namespace Web.Server.Services;

internal sealed class CurrentUserCircuitHandler
				: CircuitHandler, IDisposable
{
	private readonly AuthenticationStateProvider? _authenticationStateProvider;
	public CurrentUserCircuitHandler(CircuitServicesAccessor circuitServicesAccessor)
	{
		_authenticationStateProvider = circuitServicesAccessor.Services?.GetRequiredService<AuthenticationStateProvider>();
	}
	public override Task OnCircuitOpenedAsync(Circuit circuit,
			CancellationToken cancellationToken)
	{

		if (_authenticationStateProvider != null)
		{
			_authenticationStateProvider.AuthenticationStateChanged += AuthenticationChanged;
		}

		return base.OnCircuitOpenedAsync(circuit, cancellationToken);
	}

	private void AuthenticationChanged(Task<AuthenticationState> task)
	{
		_ = UpdateAuthentication(task);

		async Task UpdateAuthentication(Task<AuthenticationState> task)
		{
			try
			{
				var state = await task;
				//currentUser.SetUser(state.User);
				Debug.WriteLine($"CurrentUserCircuitHandler: {state.User.GetIdTokenExpiration() - DateTimeOffset.Now}");
			}
			catch
			{
			}
		}
	}

	public override async Task OnConnectionUpAsync(Circuit circuit,
			CancellationToken cancellationToken)
	{
		if (_authenticationStateProvider != null)
		{
			var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
			//currentUser.SetUser(state.User);
		}
	}

	public void Dispose()
	{
		if (_authenticationStateProvider != null)
		{
			_authenticationStateProvider.AuthenticationStateChanged -= AuthenticationChanged;
		}
	}
}