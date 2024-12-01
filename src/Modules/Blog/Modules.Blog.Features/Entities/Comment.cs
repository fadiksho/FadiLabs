using Shared.Integration.Domain;

namespace Modules.Blog.Features.Entities;

public class Comment : IEntity<Guid>, IOwnedBy
{
	public Guid Id { get; set; }
	public required string Body { get; set; }
	public DateTime CreatedDate { get; set; }
	public Guid OwndedBy { get; set; }

	public Guid PostId { get; set; }
}
