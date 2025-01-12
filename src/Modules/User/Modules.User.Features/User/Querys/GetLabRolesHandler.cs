using Modules.Shared.Integration.Models;
using Modules.User.Features.Entities;
using Modules.User.Features.Persistence;
using Shared.Features.Server.Extensions;

namespace Modules.User.Features.User.Querys;
internal class GetLabRolesHandler
	(IUserContext context) : IRequestHandler<GetLabRoles, Result<PagedList<GetLabRolesResponse>>>
{
	public async Task<Result<PagedList<GetLabRolesResponse>>> Handle(GetLabRoles request, CancellationToken cancellationToken)
	{
		var query = context.LabRoles
			.OrderBy(x => x.Name)
			.AsQueryable();

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
			AutoAssign = x.AutoAssign,
			LabsPermissions = x.LabsPermissions,
		});
	}
}
