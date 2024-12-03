using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Shared.Integration.Services;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Shared.Components.Services.Implementations;

public class PrerenderStateWithJsonOptions : IPrerenderState, IDisposable
{
	private readonly PersistingComponentStateSubscription? _subscription;
	private readonly PersistentComponentState? _persistentComponentState;
	private readonly JsonSerializerOptions _jsonSerializerOptions;
	private readonly ConcurrentDictionary<string, object?> _values = new();

	public PrerenderStateWithJsonOptions(IEnvelopMessageHandler envelopMessageHandler, PersistentComponentState? persistentComponentState = null)
	{
		_subscription = persistentComponentState?.RegisterOnPersisting(PersistAsJson, RenderMode.InteractiveAuto);
		_persistentComponentState = persistentComponentState;
		_jsonSerializerOptions = envelopMessageHandler.JsonOptions;
	}

	public async Task<T> GetValue<T>(string key, Func<Task<T>> factory)
	{
		if (_persistentComponentState?.TryTakeFromJson(
						key, out ObjectWrapper? objectWrapper) == true && objectWrapper != null)
		{
			var value = JsonSerializer.Deserialize<T>(objectWrapper.Json, _jsonSerializerOptions);

			if (value != null)
				return value;

			return await factory();
		}

		var result = await factory();
		Persist(key, result);
		return result;
	}

	void Persist<T>(string key, T value)
	{
		_values.TryRemove(key, out object? _);
		_values.TryAdd(key, value);
	}

	Task PersistAsJson()
	{
		foreach (var item in _values)
		{
			var valueAsJson = JsonSerializer.Serialize(item.Value, _jsonSerializerOptions);
			var objectWrapper = new ObjectWrapper
			{
				Json = valueAsJson
			};
			_persistentComponentState?.PersistAsJson(item.Key, objectWrapper);
		}
		return Task.CompletedTask;
	}

	public void Dispose()
	{
		_subscription?.Dispose();
	}

	private record ObjectWrapper
	{
		public required string Json { get; set; }
	}
}
