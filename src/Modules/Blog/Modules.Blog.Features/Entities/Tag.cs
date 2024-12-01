using Shared.Integration.Domain;

namespace Modules.Blog.Features.Entities;

public class Tag : IEntity<Guid>
{
	public Guid Id { get; set; }
	public required string Name { get; init; }

	public List<Post> Posts { get; set; } = [];
}