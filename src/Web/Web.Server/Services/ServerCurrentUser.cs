using Microsoft.AspNetCore.Components.Authorization;
using Shared.Features.Services;
using System.Security.Claims;

namespace Web.Server.Services;

public class ServerCurrentUser : ICurrentUser
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly CircuitServicesAccessor _circuitServicesAccessor;
	private readonly ILogger<ServerCurrentUser> _logger;

	public ServerCurrentUser(
		IHttpContextAccessor httpContextAccessor,
		CircuitServicesAccessor circuitServicesAccessor,
		ILogger<ServerCurrentUser> logger)
	{
		_httpContextAccessor = httpContextAccessor;
		_circuitServicesAccessor = circuitServicesAccessor;
		_logger = logger;
	}

	public async Task<ClaimsPrincipal> GetUser()
	{
		var authStateProvider = _circuitServicesAccessor.Services?
			.GetService<AuthenticationStateProvider>();
		// If we are in blazor server (streaming over a circuit), this provider will be non-null
		if (authStateProvider != null)
		{
			var authState = await authStateProvider.GetAuthenticationStateAsync();
			return authState.User;
		}
		// Otherwise, we should be in an SSR scenario, and the httpContext should be available
		else if (_httpContextAccessor?.HttpContext != null)
		{
			return _httpContextAccessor.HttpContext.User;
		}
		// If we are in neither blazor server or SSR, something weird is going on.
		else
		{
			_logger.LogWarning("Neither an authentication state provider or http context are available to obtain the current principal.");
			return new ClaimsPrincipal();
		}
	}

}
