using Modules.Shared.Integration.Models;
using Modules.User.Features.Entities;
using Modules.User.Features.Persistence;
using Shared.Integration.Utilities;

namespace Modules.User.Features.Querys;
internal class GetLabRolesHandler
	(IUserContext context) : IRequestHandler<GetLabRoles, Result<PagedList<GetLabRolesResponse>>>
{
	public async Task<Result<PagedList<GetLabRolesResponse>>> Handle(GetLabRoles request, CancellationToken cancellationToken)
	{
		var query = context.LabRoles.AsQueryable();

		var mappedQuery = Map(query);

		var response =
			await PagedListExtensions.CreateAsync(mappedQuery, request.PageNumber, request.PageSize);

		return response;
	}

	static IQueryable<GetLabRolesResponse> Map(IQueryable<LabRole> query)
	{
		return query.Select(x => new GetLabRolesResponse
		{
			Id = x.Id,
			Name = x.Name,
			Description = x.Description,
			Permissions = x.Permissions,
		});
	}
}
