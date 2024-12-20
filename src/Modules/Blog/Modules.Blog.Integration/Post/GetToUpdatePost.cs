namespace Modules.Blog.Integration.Post;

[LabAuthorize(LabsPermissions.BlogOwner)]
public record GetToUpdatePost(Guid PostId) : IRequest<Result<UpdatePost>>;

public class GetToUpdatePostValidator : AbstractValidator<GetToUpdatePost>
{
	public GetToUpdatePostValidator()
	{
		RuleFor(x => x.PostId)
			.NotEmpty();
	}
}