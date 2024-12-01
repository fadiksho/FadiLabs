using Modules.Shared.Integration.Models;
using Modules.Shared.Integration.Queries;

namespace Modules.Blog.Integration.Post;

public record GetCommentsByPostId(Guid PostId) : PagedFilterQuery, IRequest<Result<PagedList<GetCommentsByPostIdResponse>>>;

public record GetCommentsByPostIdResponse
{
	public Guid Id { get; set; }
	public required string Body { get; set; }
	public DateTime CreatedDate { get; set; }

	public Guid OwndedBy { get; set; }
};

public class GetCommentsByPostIdValidator : AbstractValidator<GetCommentsByPostId>
{
	public GetCommentsByPostIdValidator()
	{
		Include(new PagedFilterQueryValidator());

		RuleFor(x => x.PostId)
			.NotEmpty();
	}
}