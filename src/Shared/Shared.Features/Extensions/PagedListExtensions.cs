using Microsoft.EntityFrameworkCore;
using Modules.Shared.Integration.Models;
using Shared.Integration.Utilities;

namespace Shared.Features.Extensions;
public static class PagedListExtensions
{
	public static async Task<PagedList<T>> CreateAsync<T>(IQueryable<T> source, int pageNumber, int pageSize)
	{
		var count = await source.CountAsync();
		var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

		return new PagedList<T>(items, count, pageNumber, pageSize);
	}
}
