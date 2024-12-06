using Microsoft.EntityFrameworkCore;
using Modules.Blog.Features.Entities;
using Modules.Blog.Features.Persistence;
using Modules.Blog.Integration.Post;
using Modules.Shared.Integration.Authorization;
using Modules.Shared.Integration.Models;
using Shared.Features.Services;
using Shared.Integration.Utilities;

namespace Modules.Blog.Features.Querys;
internal class GetPostsHandler(IBlogContext context, ICurrentUser currentUser) : IRequestHandler<GetPosts, Result<PagedList<GetPostsResponse>>>
{
	public async Task<Result<PagedList<GetPostsResponse>>> Handle(GetPosts request, CancellationToken cancellationToken)
	{
		var query = context.Posts
			.Include(x => x.Tags)
			.Include(x => x.Comments)
			.OrderByDescending(x => x.PublishedDate)
			.AsQueryable();

		if (!string.IsNullOrEmpty(request.Tag))
			query = query.Where(x => x.Tags.Any(t => t.Name == request.Tag));

		if (!currentUser.HasPermission(Permissions.BlogOwner))
			query = query.Where(x => x.IsPublished);

		if (!string.IsNullOrEmpty(request.Search))
			query = query.Where(x => EF.Functions.Like(x.Title, $"%{request.Search}%"));

		var mappedQuery = Map(query);

		var response =
			await PagedListExtensions.CreateAsync(mappedQuery, request.PageNumber, request.PageSize);

		return response;
	}

	static IQueryable<GetPostsResponse> Map(IQueryable<Post> query)
	{
		return query.Select(x => new GetPostsResponse
		{
			Id = x.Id,
			Title = x.Title,
			Slug = x.Slug,
			PublishedDate = x.PublishedDate,
			Tags = x.Tags.Select(x => x.Name).ToList(),
			CommentsCount = x.Comments.Count,
			Description = x.Description,
		});
	}
}
