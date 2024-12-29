using Fadi.Result.Errors;
using Microsoft.EntityFrameworkCore;
using Modules.Blog.Features.Entities;
using Modules.Blog.Features.Persistence;
using Modules.Blog.Integration.Post;

namespace Modules.Blog.Features.Commands;
public class UpdatePostHandler(IBlogContext context) : IRequestHandler<UpdatePost, Result<UpdatePostResponse>>
{
	public async Task<Result<UpdatePostResponse>> Handle(UpdatePost request, CancellationToken cancellationToken)
	{
		var isDuplicateSlug = await context.Posts
			.AsNoTracking()
			.AnyAsync(x => x.Slug == request.Slug && x.Id != request.PostId, cancellationToken);

		if (isDuplicateSlug)
			return Result<UpdatePostResponse>.FromError(new ResultError("Already Exist"));

		var existingPost = await context.Posts
			.Include(p => p.Tags)
			.FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken);

		if (existingPost == null)
			return Result<UpdatePostResponse>.FromError(new NotFoundError());

		// Retrieve the existing tags from the database
		var existingTags = await context.Tags
			.Where(tag => request.Tags.Select(t => t.TagId).Contains(tag.Id))
			.ToListAsync(cancellationToken);

		// Identify the new tags that do not exist in the database
		var existingTagIds = existingTags.Select(t => t.Id).ToList();
		var newTags = request.Tags
			.Where(t => !existingTagIds.Contains(t.TagId))
			.Select(t => new Tag { Id = t.TagId, Name = t.Name })
			.ToList();

		// Remove tags that are no longer associated with the post
		var tagsToRemove = existingPost.Tags
			.Where(t => !request.Tags.Select(tag => tag.TagId).Contains(t.Id))
			.ToList();
		foreach (var tag in tagsToRemove)
		{
			existingPost.Tags.Remove(tag);
		}

		// Attach the existing and new tags to the post
		existingPost.Tags.AddRange(existingTags);
		existingPost.Tags.AddRange(newTags);

		var post = MapRequest(existingPost, request);

		await context.SaveChangesAsync(cancellationToken);

		var response = MapResponse(post);

		return Result<UpdatePostResponse>.FromSuccess(response);
	}

	static Post MapRequest(Post entity, UpdatePost request)
	{
		entity.Title = request.Title;
		entity.Description = request.Description;
		entity.Slug = request.Slug;
		entity.Body = request.Body;
		entity.UpdatedDate = request.UpdatedDate;
		entity.PublishedDate = request.PublishedDate;
		entity.IsPublished = request.IsPublished;

		return entity;
	}

	static UpdatePostResponse MapResponse(Post post)
	{
		return new UpdatePostResponse
		{
			PostId = post.Id,
			Slug = post.Slug,
			Title = post.Title
		};
	}
}
