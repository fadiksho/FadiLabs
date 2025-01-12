using Fadi.Result;
using MediatR;
using Modules.Blog.Features.Entities;
using Modules.Blog.Features.Persistence;
using Modules.Blog.Integration.Post;
using Modules.Shared.Integration.Models;
using Shared.Features.Server.Extensions;

namespace Modules.Blog.Features.Querys;
internal class GetCommentsByPostIdHandler : IRequestHandler<GetCommentsByPostId, Result<PagedList<GetCommentsByPostIdResponse>>>
{
	private readonly IBlogContext _context;
	public GetCommentsByPostIdHandler(IBlogContext context)
	{
		_context = context;
	}
	public async Task<Result<PagedList<GetCommentsByPostIdResponse>>> Handle(GetCommentsByPostId request, CancellationToken cancellationToken)
	{
		var query = _context
			.Comments
			.Where(x => x.PostId == request.PostId)
			.OrderByDescending(x => x.CreatedDate);

		var mappedQuery = Map(query);

		var response =
			await PagedListExtensions.CreateAsync(mappedQuery, request.PageNumber, request.PageSize);

		return response;
	}

	static IQueryable<GetCommentsByPostIdResponse> Map(IQueryable<Comment> query)
	{
		return query.Select(x => new GetCommentsByPostIdResponse
		{
			Id = x.Id,
			Body = x.Body,
			CreatedDate = x.CreatedDate,
			OwndedBy = x.OwndedBy
		});
	}
}
