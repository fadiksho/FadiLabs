using Shared.Features.Server.Services;
using Shared.Integration.Extensions;

namespace Modules.User.Features.User.Querys;
public class GetTestQueryHandler(ICurrentUser currentUser) : IRequestHandler<GetTestQuery, Result>, IRequestHandler<GetTestAuthorizedQuery, Result>
{
	private readonly ICurrentUser _currentUser = currentUser;

	public async Task<Result> Handle(GetTestQuery request, CancellationToken cancellationToken)
	{
		var user = await _currentUser.GetUser();

		return Result.FromSuccess($"Success: GetTestQuery {user.GetIdTokenExpiration() - DateTimeOffset.Now}");
	}

	public async Task<Result> Handle(GetTestAuthorizedQuery request, CancellationToken cancellationToken)
	{
		var user = await _currentUser.GetUser();

		return Result.FromSuccess($"Success: GetTestAuthorizedQuery {user.GetIdTokenExpiration() - DateTimeOffset.Now}");
	}
}
