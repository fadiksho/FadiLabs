namespace Modules.User.Features.User.Querys;
public class GetTestQueryHandler : IRequestHandler<GetTestQuery, Result>, IRequestHandler<GetTestAuthorizedQuery, Result>
{
	public Task<Result> Handle(GetTestQuery request, CancellationToken cancellationToken)
	{
		return Task.FromResult(Result.FromSuccess("Success: GetTestQuery"));
	}

	public Task<Result> Handle(GetTestAuthorizedQuery request, CancellationToken cancellationToken)
	{
		return Task.FromResult(Result.FromSuccess("Success: GetTestAuthorizedQuery"));
	}
}
