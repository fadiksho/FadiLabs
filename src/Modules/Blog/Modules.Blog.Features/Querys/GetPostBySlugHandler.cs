using Fadi.Result.Errors;
using Microsoft.EntityFrameworkCore;
using Modules.Blog.Features.Entities;
using Modules.Blog.Features.Persistence;
using Modules.Blog.Integration.Post;
using Modules.Shared.Integration.Authorization;
using Shared.Features.Server.Services;
using Shared.Integration.Extensions;

namespace Modules.Blog.Features.Querys;

internal class GetPostBySlugHandler(IBlogContext context, ICurrentUser currentUser) : IRequestHandler<GetPostBySlug, Result<GetPostBySlugResponse>>
{
	public async Task<Result<GetPostBySlugResponse>> Handle(GetPostBySlug request, CancellationToken cancellationToken)
	{
		var query = context.Posts
			.Where(x => x.Slug == request.Slug)
			.Include(x => x.Tags)
			.AsQueryable();

		var user = await currentUser.GetUser();

		if (!user.HasLabPermission(LabsPermissions.BlogOwner))
			query = query.Where(x => x.IsPublished);

		var post = await query.FirstOrDefaultAsync(cancellationToken);

		if (post is null)
			return new NotFoundError();

		var response = Map(post);

		return response;
	}

	static GetPostBySlugResponse Map(Post post) => new()
	{
		Id = post.Id,
		Title = post.Title,
		Body = post.Body,
		PublishedDate = post.PublishedDate,
		UpdatedDate = post.UpdatedDate,
		Tags = post.Tags.Select(x => x.Name).ToList(),
		IsPublished = post.IsPublished,
	};
}
