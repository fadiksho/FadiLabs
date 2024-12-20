using Microsoft.EntityFrameworkCore;

namespace Shared.Features.Persistence;

public interface IContextSeed
{
	abstract static void Seed(DbContext context, bool _);
}
