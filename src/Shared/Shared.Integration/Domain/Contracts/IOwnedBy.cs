namespace Shared.Integration.Domain;

public interface IOwnedBy
{
  Guid OwndedBy { get; set; }

  static void MarkCreatedItemAsOwnedBy<TSelf>(TSelf entity, Guid ownedById) where TSelf : IOwnedBy
  {
    entity.OwndedBy = ownedById;
  }
}
