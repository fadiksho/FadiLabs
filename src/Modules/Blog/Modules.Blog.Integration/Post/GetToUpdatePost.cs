namespace Modules.Blog.Integration.Post;

[LabAuthorize(Permissions.BlogOwner)]
public record GetToUpdatePost(Guid PostId) : IRequest<Result<UpdatePost>>;

public class GetToUpdatePostValidator : AbstractValidator<GetToUpdatePost>
{
	public GetToUpdatePostValidator()
	{
		RuleFor(x => x.PostId)
			.NotEmpty();
	}
}