namespace Modules.Blog.Integration.Post;

[LabAuthorize(Permissions.BlogOwner)]
public record CreatePost : IRequest<Result<CreatePostResponse>>
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public List<TagResponse> Tags { get; set; } = [];

	public void ResetToDefault()
	{
		Title = string.Empty;
		Description = null;
		Tags = [];
	}
}

public record CreatePostResponse
{
	public Guid PostId { get; set; }
	public required string Title { get; set; }
	public required string Slug { get; set; }
}

public class CreatePostValidator : AbstractValidator<CreatePost>
{
	public CreatePostValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty();
	}
}