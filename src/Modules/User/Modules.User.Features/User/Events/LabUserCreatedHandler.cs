using Microsoft.Extensions.Logging;

namespace Modules.User.Features.User.Events;
internal class LabUserCreatedHandler
	(ILogger<LabUserCreated> logger) : INotificationHandler<LabUserCreated>
{
	private readonly ILogger<LabUserCreated> _logger = logger;
	public Task Handle(LabUserCreated notification, CancellationToken cancellationToken)
	{
		_logger.LogInformation($"User Created {notification.LabUserId}.");

		return Task.CompletedTask;
	}
}

public record LabUserCreated(Guid LabUserId) : INotification;