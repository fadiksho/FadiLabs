using Microsoft.AspNetCore.Http;
using Modules.Shared.Integration.Authorization;
using Shared.Integration.Extensions;
using System.Diagnostics;
using System.Security.Claims;

namespace Shared.Features.Services.Implementaions;

public class ServerCurrentUser(
	IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	public bool IsAuthenticated()
		=> GetHttpContext().User.IsAuthenticated();

	public ClaimsPrincipal GetUser()
		=> GetHttpContext().User;

	public string? GetUserId()
		=> GetHttpContext().User.GetUserId();

	public string? GetUserName()
		=> GetHttpContext().User.GetUserName();

	public LabsPermissions GetUserPermissions()
		=> GetHttpContext().User.GetPermissions();

	public bool HasLabPermission(LabsPermissions permission)
		=> GetUser().HasLabPermission(permission);

	private HttpContext GetHttpContext()
	{
		if (_httpContextAccessor.HttpContext == null)
			throw new UnreachableException($"HttpContext is not set in {nameof(ServerCurrentUser)}().");

		return _httpContextAccessor.HttpContext;
	}
}
