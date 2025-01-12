using Microsoft.EntityFrameworkCore;
using Modules.Shared.Integration.Models;
using Modules.User.Features.Entities;
using Modules.User.Features.Persistence;
using Shared.Features.Server.Extensions;

namespace Modules.User.Features.User.Querys;

internal class GetLabUsersHandler
	(IUserContext context) : IRequestHandler<GetLabUsers, Result<PagedList<GetLabUsersResponse>>>
{
	public async Task<Result<PagedList<GetLabUsersResponse>>> Handle(GetLabUsers request, CancellationToken cancellationToken)
	{
		var query = context.LabUsers
			.Include(x => x.LabRoles)
			.OrderByDescending(x => x.Id)
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
			LabUserId = x.Id,
			Email = x.Email,
			Auth0UserId = x.Auth0UserId,
			DisplayName = x.DisplayName,
			EmailVerified = x.EmailVerified,
			ProfilePictureUrl = x.ProfilePictureUrl,
			Roles = x.LabRoles.Select(x => x.Name).ToList()
		});
	}
}
