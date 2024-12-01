namespace Shared.Integration.Domain;

public abstract class BaseEntity : IEntity<Guid>, IBaseEntity
{
  public Guid Id { get; set; }

  private readonly IList<EntityEvent> _entityEvents;

  public IReadOnlyCollection<EntityEvent> EntityEvents => _entityEvents.AsReadOnly();

  protected BaseEntity()
  {
    Id = Guid.NewGuid();
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
