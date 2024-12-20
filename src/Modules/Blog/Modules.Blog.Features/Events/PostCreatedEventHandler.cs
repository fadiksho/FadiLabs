using Microsoft.Extensions.Logging;
using Modules.Blog.Integration.Post;

namespace Modules.Blog.Features.Events;

public class PostCreatedHandler(ILogger<PostCreatedHandler> logger) : INotificationHandler<PostCreatedEvent>
{
	private readonly ILogger<PostCreatedHandler> _logger = logger;
	public Task Handle(PostCreatedEvent notification, CancellationToken cancellationToken)
	{
		_logger.LogInformation($"Post Created {notification.PostId}.");

		return Task.CompletedTask;
	}
}