namespace Modules.Shared.Integration.Authorization;

/// <summary>
/// Provides helper methods for generating and validating policy names based on permissions.
/// </summary>
public static class LabsPermissionsPolicyManager
{
	/// <summary>
	/// The prefix used for policy names.
	/// </summary>
	public const string PolicyPrefix = "LabsPermissions";

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
	/// <param name="labsPermissions">The permissions to generate a policy name for.</param>
	/// <returns>The generated policy name.</returns>
	public static string GeneratePolicyName(LabsPermissions labsPermissions)
	{
		return $"{PolicyPrefix}{(int)labsPermissions}";
	}

	/// <summary>
	/// Retrieves the permissions from the given policy name.
	/// </summary>
	/// <param name="policyName">The policy name to extract permissions from.</param>
	/// <returns>The extracted permissions.</returns>
	public static LabsPermissions ExtractLabsPermissions(string policyName)
	{
		var labsPermissionsValue = int.Parse(policyName[PolicyPrefix.Length..]!);
		return (LabsPermissions)labsPermissionsValue;
	}

	/// <summary>
	/// Converts a list of LabsPermissions strings to a combined LabsPermissions enum value.
	/// </summary>
	/// <param name="labsPermissionsValues">A list of permission strings.</param>
	/// <returns>A combined LabsPermissions enum value representing all the LabsPermissions in the list.</returns>
	public static LabsPermissions ConvertToLabsPermissions(IEnumerable<string> labsPermissionsValues)
	{
		// Initialize the permissions variable with no permissions.
		LabsPermissions labsPermissions = LabsPermissions.None;

		foreach (var value in labsPermissionsValues)
		{
			if (Enum.TryParse(typeof(LabsPermissions), value, out var result))
			{
				// Combine the parsed permission with the existing permissions using the bitwise OR operator.
				labsPermissions |= (LabsPermissions)result;
			}
		}

		return labsPermissions;
	}

	/// <summary>
	/// Retrieves all possible permissions.
	/// </summary>
	/// <returns>An array of all possible permissions.</returns>
	public static LabsPermissions[] GetAllLabsPermissions()
	{
		return Enum.GetValues<LabsPermissions>();
	}
}

