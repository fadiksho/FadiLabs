using Fadi.Result;
using MediatR;
using Modules.Blog.Features.Entities;
using Modules.Blog.Features.Persistence;
using Modules.Blog.Integration.Post;
using Modules.Shared.Integration.Models;
using Shared.Features.Extensions;

namespace Modules.Blog.Features.Querys;
public class GetTagsHandler : IRequestHandler<GetTags, Result<PagedList<TagResponse>>>
{
	private IBlogContext _context;

	public GetTagsHandler(IBlogContext context)
	{
		_context = context;
	}

	public async Task<Result<PagedList<TagResponse>>> Handle(GetTags request, CancellationToken cancellationToken)
	{
		var query = _context.Tags
			.OrderBy(x => x.Name)
			.AsQueryable();

		var mappedQuery = Map(query);

		var response =
			await PagedListExtensions.CreateAsync(mappedQuery, request.PageNumber, request.PageSize);

		return response;
	}

	static IQueryable<TagResponse> Map(IQueryable<Tag> tags)
	{
		return tags.Select(x => new TagResponse { Name = x.Name, TagId = x.Id });
	}

}
