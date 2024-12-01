using Shared.Features.Persistence;

namespace Web.Server.Middlewares;

public class SeedContextMiddleware
{
	private readonly RequestDelegate _next;
	private IEnumerable<IContextSeed> _contextsSeed;
	private bool _alreadyRan = false;
	public SeedContextMiddleware(
		RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, IEnumerable<IContextSeed> contextSeeds)
	{
		if (!_alreadyRan)
		{
			_alreadyRan = true;
			_contextsSeed = contextSeeds;

			foreach (var seedContext in _contextsSeed)
			{
				await seedContext.SeedAsync();
			}
		}

		await _next(context);
	}
}
