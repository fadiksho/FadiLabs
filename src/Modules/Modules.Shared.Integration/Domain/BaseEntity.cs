using Modules.Shared.Integration.Domain.Contracts;

namespace Modules.Shared.Integration.Domain;

public abstract class BaseEntity : IBaseEntity
{
	private readonly IList<EntityEvent> _entityEvents;

	public IReadOnlyCollection<EntityEvent> EntityEvents => _entityEvents.AsReadOnly();

	protected BaseEntity()
	{
		_entityEvents = [];
	}

	public void AddEntityEvent(EntityEvent @event)
	{
		_entityEvents.Add(@event);
	}

	public void RemoveEntityEvent(EntityEvent @event)
	{
		_entityEvents.Remove(@event);
	}

	public void ClearEntityEvents()
	{
		_entityEvents.Clear();
	}
}
