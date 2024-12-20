using Microsoft.EntityFrameworkCore;
using Modules.User.Features.Persistence;

namespace Modules.User.Features.User.Events;

internal class FirstLabUserCreatedHandler
	(IUserContext context) : INotificationHandler<FirstLabUserCreated>
{
	public async Task Handle(FirstLabUserCreated notification, CancellationToken cancellationToken)
	{
		var user = await context.LabUsers
			.FirstAsync(cancellationToken);

		var role = await context.LabRoles
			.FirstAsync(x => x.Name == "admin", cancellationToken);

		user.LabRoles.Add(role);
		await context.SaveChangesAsync(cancellationToken);
	}
}
