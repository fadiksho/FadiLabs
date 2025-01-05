using Microsoft.EntityFrameworkCore;
using Modules.Blog.Features.Entities;
using Modules.Blog.Features.Persistence;
using Modules.Blog.Integration.Post;
using Modules.Shared.Integration.Authorization;
using Modules.Shared.Integration.Models;
using Shared.Features.Extensions;
using Shared.Features.Services;
using Shared.Integration.Extensions;
using System.Linq.Expressions;

namespace Modules.Blog.Features.Querys;
internal class GetPostsHandler(IBlogContext context, ICurrentUser currentUser) : IRequestHandler<GetPosts, Result<PagedList<GetPostsResponse>>>
{
	public async Task<Result<PagedList<GetPostsResponse>>> Handle(GetPosts request, CancellationToken cancellationToken)
	{
		await Task.Delay(500);
		var query = context.Posts
			.Include(x => x.Tags)
			.Include(x => x.Comments)
			//.OrderByDescending(x => x.PublishedDate)
			.AsQueryable();

		query = !string.IsNullOrEmpty(request.SortBy)
			? SortByProperty(query, request.SortBy, request.Descending)
			: SortByProperty(query, nameof(Post.Title), request.Descending);

		if (!string.IsNullOrEmpty(request.Tag))
			query = query.Where(x => x.Tags.Any(t => t.Name == request.Tag));

		var user = await currentUser.GetUser();

		if (!user.HasLabPermission(LabsPermissions.BlogOwner))
			query = query.Where(x => x.IsPublished);

		//if (!string.IsNullOrEmpty(request.Search))
		//	query = query.Where(x => EF.Functions.Like(x.Title, $"%{request.Search}%"));

		if (!string.IsNullOrEmpty(request.Search))
			query = query.Where(x => x.Title.Contains(request.Search));

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

	public static IQueryable<T> SortByProperty<T>(IQueryable<T> source, string propertyName, bool descending)
	{
		var param = Expression.Parameter(typeof(T), "p");
		var property = Expression.Property(param, propertyName)
				?? throw new ArgumentException($"Property '{propertyName}' does not exist on type '{typeof(T).Name}'");
		var sortExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);

		return descending
			? source.OrderByDescending(sortExpression)
			: source.OrderBy(sortExpression);
	}
}
