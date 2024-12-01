using Fadi.Result;
using Fadi.Result.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Modules.Blog.Features.Persistence;
using Modules.Blog.Integration.Post;

namespace Modules.Blog.Features.Commands;
internal class DeletePostByIdHandler : IRequestHandler<DeletePostById, Result>
{
	private readonly IBlogContext _context;
	public DeletePostByIdHandler(IBlogContext context)
	{
		_context = context;
	}

	public async Task<Result> Handle(DeletePostById request, CancellationToken cancellationToken)
	{
		var post = await _context.Posts
			.Where(x => x.Id == request.Id)
			.FirstOrDefaultAsync(cancellationToken);

		if (post is null)
			return Result.FromError(new NotFoundError());

		_context.Posts.Remove(post);
		await _context.SaveChangesAsync(cancellationToken);

		return Result.FromSuccess($"Post: {post.Title} Deleted.");
	}
}
