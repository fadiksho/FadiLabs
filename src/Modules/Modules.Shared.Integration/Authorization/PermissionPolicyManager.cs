namespace Modules.Shared.Integration.Authorization;

/// <summary>
/// Provides helper methods for generating and validating policy names based on permissions.
/// </summary>
public static class PermissionPolicyManager
{
	/// <summary>
	/// The prefix used for policy names.
	/// </summary>
	public const string PolicyPrefix = "Permissions";

	/// <summary>
	/// Validates if the given policy name is a valid permission policy name.
	/// </summary>
	/// <param name="policyName">The policy name to validate.</param>
	/// <returns>True if the policy name is valid; otherwise, false.</returns>
	public static bool IsValidPolicyName(string? policyName)
	{
		return policyName != null && policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase);
	}

	/// <summary>
	/// Generates a policy name for the specified permissions.
	/// </summary>
	/// <param name="permissions">The permissions to generate a policy name for.</param>
	/// <returns>The generated policy name.</returns>
	public static string GeneratePolicyName(Permissions permissions)
	{
		return $"{PolicyPrefix}{(int)permissions}";
	}

	/// <summary>
	/// Retrieves the permissions from the given policy name.
	/// </summary>
	/// <param name="policyName">The policy name to extract permissions from.</param>
	/// <returns>The extracted permissions.</returns>
	public static Permissions ExtractPermissions(string policyName)
	{
		var permissionsValue = int.Parse(policyName[PolicyPrefix.Length..]!);
		return (Permissions)permissionsValue;
	}

	/// <summary>
	/// Converts a list of permission strings to a combined Permissions enum value.
	/// </summary>
	/// <param name="values">A list of permission strings.</param>
	/// <returns>A combined Permissions enum value representing all the permissions in the list.</returns>
	public static Permissions ConvertToLabsPermissions(IEnumerable<string> values)
	{
		// Initialize the permissions variable with no permissions.
		Permissions permissions = Permissions.None;

		foreach (var value in values)
		{
			if (Enum.TryParse(typeof(Permissions), value, out var result))
			{
				// Combine the parsed permission with the existing permissions using the bitwise OR operator.
				permissions |= (Permissions)result;
			}
		}

		return permissions;
	}

	/// <summary>
	/// Retrieves all possible permissions.
	/// </summary>
	/// <returns>An array of all possible permissions.</returns>
	public static Permissions[] GetAllPermissions()
	{
		return Enum.GetValues<Permissions>();
	}
}

