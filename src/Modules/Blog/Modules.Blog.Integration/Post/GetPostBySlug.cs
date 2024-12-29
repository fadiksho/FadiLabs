namespace Modules.Blog.Integration.Post;

public record GetPostBySlug(string Slug) : IRequest<Result<GetPostBySlugResponse>>;

public record GetPostBySlugResponse
{
	public Guid Id { get; set; }

	public required string Title { get; init; }
	public string? Body { get; set; }
	public DateTime? PublishedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public List<string> Tags { get; set; } = [];

	public bool IsPublished { get; set; }
}

public class GetPostBySlugValidator : AbstractValidator<GetPostBySlug>
{
	public GetPostBySlugValidator()
	{
		RuleFor(x => x.Slug)
			.NotEmpty();
	}
}