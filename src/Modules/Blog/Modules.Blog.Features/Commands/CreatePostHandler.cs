using Fadi.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Modules.Blog.Features.Entities;
using Modules.Blog.Features.Persistence;
using Modules.Blog.Integration.Post;

namespace Modules.Blog.Features.Commands;
public class CreatePostHandler(IBlogContext context) : IRequestHandler<CreatePost, Result<CreatePostResponse>>
{
	public async Task<Result<CreatePostResponse>> Handle(CreatePost request, CancellationToken cancellationToken)
	{
		var postSlug = Post.CreateSlug(request.Title);

		var isDuplicateSlug = await context.Posts
			.AnyAsync(x => x.Slug == postSlug, cancellationToken);

		if (isDuplicateSlug)
			return Result<CreatePostResponse>.FromError(new ResultError("Already Exist"));

		var newPost = MapRequest(request, postSlug);

		// Retrieve the existing tags from the database
		var existingTags = await context.Tags
			.Where(tag => request.Tags.Select(t => t.TagId)
					.Contains(tag.Id))
			.ToListAsync(cancellationToken);

		// Identify the new tags that do not exist in the database
		var existingTagIds = existingTags.Select(t => t.Id).ToList();
		var newTags = request.Tags
			.Where(t => !existingTagIds.Contains(t.TagId))
			.Select(t => new Tag { Id = t.TagId, Name = t.Name })
			.ToList();

		// Attach the existing and new tags to the new post
		newPost.Tags.AddRange(existingTags);
		newPost.Tags.AddRange(newTags);

		context.Posts.Add(newPost);
		await context.SaveChangesAsync(cancellationToken);

		var response = MapResponse(newPost);

		return Result<CreatePostResponse>.FromSuccess(response);
	}

	static Post MapRequest(CreatePost post, string slug)
	{
		return new Post
		{
			Title = post.Title,
			Slug = slug,
			Description = post.Description,
		};
	}

	static CreatePostResponse MapResponse(Post post)
	{
		return new CreatePostResponse
		{
			PostId = post.Id,
			Slug = post.Slug,
			Title = post.Title
		};
	}
}
