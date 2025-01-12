namespace Shared.Features.Server.Persistence;

public interface IDbContext
{
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
