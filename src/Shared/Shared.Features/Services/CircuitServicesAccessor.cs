namespace Shared.Features.Services;

public class CircuitServicesAccessor
{
	static readonly AsyncLocal<IServiceProvider> _blazorServices = new();

	public IServiceProvider? Services
	{
		get => _blazorServices.Value;
		set => _blazorServices.Value = value!;
	}
}