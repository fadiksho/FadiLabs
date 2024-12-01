namespace Shared.Integration.Domain;

public interface IEntity<TEntityId> : IEntity
{
  public TEntityId Id { get; set; }
}

public interface IEntity
{
}
