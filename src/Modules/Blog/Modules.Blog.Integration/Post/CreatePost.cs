namespace Modules.Blog.Integration.Post;

[LabAuthorize(LabsPermissions.BlogOwner)]
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

	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var result = await ValidateAsync(ValidationContext<CreatePost>.CreateWithOptions((CreatePost)model, x => x.IncludeProperties(propertyName)));
		if (result.IsValid)
			return [];

		return result.Errors.Select(e => e.ErrorMessage);
	};
}