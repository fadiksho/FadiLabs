using Modules.Shared.Integration.Models;
using Modules.Shared.Integration.Queries;

namespace Modules.Blog.Integration.Post;

public record GetPosts : PagedFilterQuery, IRequest<Result<PagedList<GetPostsResponse>>>
{
	public string? Tag { get; set; }
	public string? Search { get; set; }
	public string? SortBy { get; set; }
	public bool Descending { get; set; }
}

public class GetPostsResponse
{
	public Guid Id { get; set; }
	public string? Description { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Slug { get; set; } = string.Empty;
	public DateTime? PublishedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public List<string> Tags { get; set; } = [];
	public int CommentsCount { get; set; }
}

public class GetPostsValidator : AbstractValidator<GetPosts>
{
	public GetPostsValidator()
	{
		Include(new PagedFilterQueryValidator());

		RuleFor(x => x.Tag)
			.MaximumLength(50);

		RuleFor(x => x.Search)
			.MaximumLength(50);
	}
}