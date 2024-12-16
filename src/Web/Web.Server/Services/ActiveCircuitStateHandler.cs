using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Web.Server.Services;

public class ActiveCircuitStateHandler(ActiveCircuitState activeCircuitState) : CircuitHandler
{
	private readonly ActiveCircuitState _activeCircuitState = activeCircuitState;

	public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
	{
		_activeCircuitState.CircuitExists = true;
		return base.OnCircuitOpenedAsync(circuit, cancellationToken);
	}
}
