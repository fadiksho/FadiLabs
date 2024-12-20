namespace Modules.Shared.Integration.Domain.Contracts;

public interface IOwnedBy
{
	Guid OwndedBy { get; set; }

	static void MarkCreatedItemAsOwnedBy<TSelf>(TSelf entity, Guid ownedById) where TSelf : IOwnedBy
	{
		entity.OwndedBy = ownedById;
	}
}
