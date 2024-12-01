using Modules.Shared.Integration.Authorization;

namespace Modules.Authorization.Integration.Authorization;

/// <summary>
/// Provides methods to retrieve permissions.
/// </summary>
public static class PermissionsProvider
{
	/// <summary>
	/// Retrieves all permissions defined in the Permissions enum.
	/// </summary>
	/// <returns>A list of all permissions.</returns>
	public static List<Permissions> GetAll()
	{
		return [.. Enum.GetValues<Permissions>()];
	}
}
