namespace Shared.Components.Services.Implementations;
public class ServiceExecutor<T> : IServiceExecutor<T> where T : class
{
	private readonly T _service;
	private bool _isLoading;
	private readonly Dictionary<string, bool> _loadingStates = [];

	private readonly IPrerenderState _prerenderState;
	public ServiceExecutor(T service, IPrerenderState prerenderState)
	{
		_service = service;
		_prerenderState = prerenderState;
	}

	public bool IsLoading => _isLoading;

	public Dictionary<string, bool> LoadingStates => _loadingStates;

	public async Task<TReturn> ExecuteAsync<TReturn>(string requestKey, Func<T, Task<TReturn>> action, bool persistDuringPrerender = false)
	{
		try
		{
			_isLoading = true;
			_loadingStates[requestKey] = true;

			if (persistDuringPrerender)
			{
				return await _prerenderState.GetValue(requestKey, async () => await action(_service));
			}

			return await action(_service);
		}
		finally
		{
			_isLoading = false;
			_loadingStates[requestKey] = false;
		}
	}

	public async Task<TReturn> ExecuteAsync<TReturn>(Func<T, Task<TReturn>> action)
	{
		try
		{
			_isLoading = true;

			return await action(_service);
		}
		finally
		{
			_isLoading = false;
		}
	}

	public bool IsRequestLoading(string requestKey)
	{
		return _loadingStates.TryGetValue(requestKey, out bool isLoading) && isLoading;
	}
}

