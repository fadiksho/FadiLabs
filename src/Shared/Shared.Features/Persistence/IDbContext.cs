namespace Shared.Features.Persistence;

public interface IDbContext
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
