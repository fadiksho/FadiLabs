namespace Modules.Blog.Integration.Post;

[LabAuthorize(LabsPermissions.BlogOwner)]
public record UpdatePost : IRequest<Result<UpdatePostResponse>>
{
	public required Guid PostId { get; set; }
	public required string Title { get; set; }
	public required string Slug { get; set; }
	public string? Description { get; set; }
	public string? Body { get; set; }
	public DateTime PublishedDate { get; set; }
	public DateTime UpdatedDate { get; set; }
	public bool IsPublished { get; set; }
	public List<TagResponse> Tags { get; set; } = [];
}

public record UpdatePostResponse
{
	public Guid PostId { get; set; }
	public required string Title { get; set; }
	public required string Slug { get; set; }
}


public class UpdatePostValidator : AbstractValidator<UpdatePost>
{
	public UpdatePostValidator()
	{
		RuleFor(x => x.PostId)
			.NotEmpty();

		RuleFor(x => x.Title)
			.NotEmpty();

		RuleFor(x => x.Slug)
			.NotEmpty();

		RuleFor(x => x.Description)
			.NotEmpty()
			.When(x => x.IsPublished);

		RuleFor(x => x.UpdatedDate)
			.GreaterThanOrEqualTo(x => x.PublishedDate);

		RuleFor(x => x.Body)
			.NotEmpty()
			.When(x => x.IsPublished);

		RuleFor(x => x.PublishedDate)
			.NotEmpty()
			.When(x => x.IsPublished);
	}
}
