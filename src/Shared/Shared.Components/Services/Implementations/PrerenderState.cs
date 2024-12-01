
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Concurrent;

namespace Shared.Components.Services.Implementations;
public class PrerenderState : IPrerenderState, IDisposable
{
	private readonly PersistingComponentStateSubscription? _subscription;
	private readonly PersistentComponentState? _persistentComponentState;
	private readonly ConcurrentDictionary<string, object?> values = new();

	public PrerenderState(PersistentComponentState? persistentComponentState = null)
	{
		_subscription = persistentComponentState?.RegisterOnPersisting(PersistAsJson, RenderMode.InteractiveServer);
		_persistentComponentState = persistentComponentState;
	}

	public async Task<T> GetValue<T>(string key, Func<Task<T>> factory)
	{
		if (_persistentComponentState?.TryTakeFromJson(
						key, out T? value) == true && value != null)
		{
			return value;
		}

		var result = await factory();
		Persist(key, result);
		return result;
	}

	void Persist<T>(string key, T value)
	{
		values.TryRemove(key, out object? _);
		values.TryAdd(key, value);
	}

	Task PersistAsJson()
	{
		foreach (var item in values)
		{
			_persistentComponentState?.PersistAsJson(item.Key, item.Value);
		}
		return Task.CompletedTask;
	}

	public void Dispose()
	{
		_subscription?.Dispose();
	}
}
