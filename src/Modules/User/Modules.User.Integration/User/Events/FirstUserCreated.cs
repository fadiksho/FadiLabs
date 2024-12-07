namespace Modules.User.Integration.User.Events;

public record FirstLabUserCreated(Guid LabUserId) : INotification { }
