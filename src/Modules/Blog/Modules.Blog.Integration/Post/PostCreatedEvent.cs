using Modules.Shared.Integration.Domain;

namespace Modules.Blog.Integration.Post;

public class PostCreatedEvent(ref Guid postId) : EntityEvent
{
	public Guid PostId { get; private set; } = postId;
}