using Modules.Shared.Integration.Models;
using Modules.Shared.Integration.Queries;

namespace Modules.Blog.Integration.Post;
public record GetTags() : PagedFilterQuery, IRequest<Result<PagedList<TagResponse>>>;

public record TagResponse
{
	public Guid TagId { get; set; }
	public required string Name { get; set; }
}