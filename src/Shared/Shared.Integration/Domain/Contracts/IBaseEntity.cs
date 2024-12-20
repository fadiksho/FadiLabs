namespace Shared.Integration.Domain.Contracts;

public interface IBaseEntity
{
	public IReadOnlyCollection<EntityEvent> EntityEvents { get; }

	public void AddEntityEvent(EntityEvent @event);

	public void RemoveEntityEvent(EntityEvent @event);

	public void ClearEntityEvents();
}
