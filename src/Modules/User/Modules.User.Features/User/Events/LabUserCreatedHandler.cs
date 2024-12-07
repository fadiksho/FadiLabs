
using Microsoft.EntityFrameworkCore;
using Modules.User.Features.Persistence;

namespace Modules.User.Features.User.Events;
internal class LabUserCreatedHandler
	(IUserContext context) : INotificationHandler<LabUserCreated>
{
	public async Task Handle(LabUserCreated notification, CancellationToken cancellationToken)
	{
		var user = await context.LabUsers
			.FirstAsync(x => x.Id == notification.LabUserId, cancellationToken);

		var role = await context.LabRoles
			.FirstAsync(x => x.Name == "default lab role", cancellationToken);

		user.LabRoles.Add(role);
		await context.SaveChangesAsync(cancellationToken);
	}
}

public record LabUserCreated(Guid LabUserId) : INotification;