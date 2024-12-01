namespace Shared.Components.Services;
/// <summary>
/// Defines the contract for executing service actions with loading state management.
/// </summary>
/// <typeparam name="T">The type of the service.</typeparam>
public interface IServiceExecutor<T> where T : class
{
	/// <summary>
	/// Gets a value indicating whether any request is currently loading.
	/// </summary>
	bool IsLoading { get; }

	/// <summary>
	/// Gets the dictionary that tracks the loading states of specific requests.
	/// </summary>
	Dictionary<string, bool> LoadingStates { get; }

	/// <summary>
	/// Executes an asynchronous action with a specified request key and optionally persists the state during prerendering.
	/// </summary>
	/// <typeparam name="TReturn">The return type of the action.</typeparam>
	/// <param name="requestKey">The key to identify the request.</param>
	/// <param name="action">The action to execute.</param>
	/// <param name="persistDuringPrerender">Indicates whether to persist the state during prerendering.</param>
	/// <returns>The result of the action.</returns>
	Task<TReturn> ExecuteAsync<TReturn>(string requestKey, Func<T, Task<TReturn>> action, bool persistDuringPrerender = false);

	/// <summary>
	/// Executes an asynchronous action without a specified request key.
	/// </summary>
	/// <typeparam name="TReturn">The return type of the action.</typeparam>
	/// <param name="action">The action to execute.</param>
	/// <returns>The result of the action.</returns>
	Task<TReturn> ExecuteAsync<TReturn>(Func<T, Task<TReturn>> action);

	/// <summary>
	/// Checks if a specific request is currently loading.
	/// </summary>
	/// <param name="requestKey">The key to identify the request.</param>
	/// <returns>True if the request is loading, otherwise false.</returns>
	bool IsRequestLoading(string requestKey);
}

