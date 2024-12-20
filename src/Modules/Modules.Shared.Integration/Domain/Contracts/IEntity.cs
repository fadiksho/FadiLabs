namespace Modules.Shared.Integration.Domain.Contracts;

public interface IEntity<TEntityId> : IEntity
{
	public TEntityId Id { get; set; }
}

public interface IEntity
{
}
