namespace Shared.Components.Services;

public interface IPrerenderState
{
	Task<T> GetValue<T>(string key, Func<Task<T>> factory);
}