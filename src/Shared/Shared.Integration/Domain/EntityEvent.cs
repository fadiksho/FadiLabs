using MediatR;
using Shared.Integration.Utilities;

namespace Shared.Integration.Domain;

public abstract class EntityEvent : INotification
{
  // ToDo: Generic Id can be used here
  public Guid Id { get; protected set; }
  public string MessageType { get; protected set; }
  public DateTime Timestamp { get; private set; }
  public string? EventDescription { get; set; }

  protected EntityEvent(string? description = null)
  {
    Timestamp = DateTime.Now;
    EventDescription = description;
    MessageType = GetType().GetGenericTypeName();
  }
}
