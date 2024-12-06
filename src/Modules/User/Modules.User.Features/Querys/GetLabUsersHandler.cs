using Microsoft.EntityFrameworkCore;
using Modules.Shared.Integration.Models;
using Modules.Shared.Integration.Queries;
using Modules.User.Features.Entities;
using Modules.User.Features.Persistence;
using Shared.Integration.Utilities;

namespace Modules.User.Features.Querys;

internal class GetLabUsersHandler
	(IUserContext context) : IRequestHandler<GetLabUsers, Result<PagedList<GetLabUsersResponse>>>
{
	public async Task<Result<PagedList<GetLabUsersResponse>>> Handle(GetLabUsers request, CancellationToken cancellationToken)
	{
		var query = context.LabUsers
			.Include(x => x.LabRoles)
			.AsQueryable();

		var mappedQuery = Map(query);

		var response =
			await PagedListExtensions.CreateAsync(mappedQuery, request.PageNumber, request.PageSize);

		return response;
	}

	static IQueryable<GetLabUsersResponse> Map(IQueryable<LabUser> query)
	{
		return query.Select(x => new GetLabUsersResponse
		{
			Email = x.Email,
			ProfilePictureUrl = x.ProfilePictureUrl,
			Roles = x.LabRoles.Select(x => x.Name).ToList()
		});
	}
}

public record GetLabUsers : PagedFilterQuery, IRequest<Result<PagedList<GetLabUsersResponse>>>;

public record GetLabUsersResponse
{
	public required string Email { get; set; }
	public string? ProfilePictureUrl { get; set; }

	public List<string> Roles { get; set; } = [];
}