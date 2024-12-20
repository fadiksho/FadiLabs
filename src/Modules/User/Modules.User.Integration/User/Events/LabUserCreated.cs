using Modules.Shared.Integration.Domain;

namespace Modules.User.Integration.User.Events;

public class LabUserCreated(Guid labUserId) : EntityEvent
{
	public Guid LabUserId { get; private set; } = labUserId;
};
