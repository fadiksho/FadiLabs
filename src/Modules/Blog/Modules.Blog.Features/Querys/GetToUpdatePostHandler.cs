using Fadi.Result;
using Fadi.Result.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Modules.Blog.Features.Entities;
using Modules.Blog.Features.Persistence;
using Modules.Blog.Integration.Post;

namespace Modules.Blog.Features.Querys;
public class GetToUpdatePostHandler(IBlogContext context) : IRequestHandler<GetToUpdatePost, Result<UpdatePost>>
{
	public async Task<Result<UpdatePost>> Handle(GetToUpdatePost request, CancellationToken cancellationToken)
	{
		var post = await context.Posts
			.Include(x => x.Tags)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken);

		if (post == null)
			return Result<UpdatePost>.FromError(new NotFoundError());

		var result = Map(post);

		return result;
	}

	static UpdatePost Map(Post query)
	{
		return new UpdatePost
		{
			PostId = query.Id,
			Title = query.Title,
			Description = query.Description,
			Body = query.Body,
			Slug = query.Slug,
			Tags = query.Tags.Select(x => new TagResponse { Name = x.Name, TagId = x.Id }).ToList(),
			IsPublished = query.IsPublished,
			PublishedDate = query.PublishedDate,
			UpdatedDate = query.UpdatedDate
		};
	}
}
