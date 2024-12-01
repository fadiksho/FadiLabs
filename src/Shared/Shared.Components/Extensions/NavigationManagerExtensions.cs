using Microsoft.AspNetCore.Components;

namespace Shared.Components.Extensions;

/// <summary>
/// Provides extension methods for the NavigationManager class.
/// </summary>
public static class NavigationManagerExtensions
{
	/// <summary>
	/// Returns a URI that is constructed by updating <see cref="NavigationManager.Uri"/> without query string parameters.
	/// </summary>
	/// <param name="navigationManager">The <see cref="NavigationManager"/>.</param>
	public static string GetUriWithoutQueryParameters(this NavigationManager navigation)
	{
		var uri = new UriBuilder(navigation.Uri)
		{
			Query = string.Empty
		};

		return uri.ToString();
	}

	/// <summary>
	/// Gets the base URI with the specified path.
	/// </summary>
	/// <param name="navigation">The NavigationManager instance.</param>
	/// <param name="path">The path to include in the base URI.</param>
	/// <returns>The base URI with the specified path.</returns>
	public static string GetBaseUriWithPath(this NavigationManager navigation, string path)
	{
		// Get the base URI from the NavigationManager
		var baseUri = navigation.BaseUri;

		// Ensure the base URI ends with a slash
		if (!baseUri.EndsWith('/'))
		{
			baseUri += "/";
		}

		// Remove the leading slash from the path if it exists
		if (path.StartsWith('/'))
		{
			path = path.TrimStart('/');
		}

		// Combine the base URI with the specified path
		return $"{baseUri}{path}";
	}
}
