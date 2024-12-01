using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Modules.Shared.Integration.Authorization;
using Shared.Integration.Extensions;
using System.Diagnostics;
using System.Security.Claims;

namespace Shared.Features.Services.Implementaions;

public class ServerCurrentUser : ICurrentUser
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly ILogger<ServerCurrentUser> _logger;

	public ServerCurrentUser(
		IHttpContextAccessor httpContextAccessor,
		ILogger<ServerCurrentUser> logger)
	{
		_httpContextAccessor = httpContextAccessor;
		_logger = logger;
	}

	public ClaimsPrincipal GetUser()
	{
		if (_httpContextAccessor.HttpContext == null)
			throw new UnreachableException($"HttpContext is not set in {nameof(ServerCurrentUser)}().");

		return _httpContextAccessor.HttpContext.User;
	}

	public string? GetUserId()
	{
		if (_httpContextAccessor.HttpContext == null)
			throw new UnreachableException($"HttpContext is not set in {nameof(ServerCurrentUser)}().");

		return _httpContextAccessor.HttpContext.User.GetUserId();
	}

	public string? GetUserName()
	{
		if (_httpContextAccessor.HttpContext == null)
			throw new UnreachableException($"HttpContext is not set in {nameof(ServerCurrentUser)}().");

		return _httpContextAccessor.HttpContext.User.GetUserName();
	}

	public Permissions GetUserPermissions()
	{
		if (_httpContextAccessor.HttpContext == null)
			throw new UnreachableException($"HttpContext is not set in {nameof(ServerCurrentUser)}().");

		return _httpContextAccessor.HttpContext.User.GetPermissions();
	}

	public bool HasPermission(Permissions permission)
	{
		return GetUser().HasPermission(permission);
	}
}
