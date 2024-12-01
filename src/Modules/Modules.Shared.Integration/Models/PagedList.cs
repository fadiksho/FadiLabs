namespace Modules.Shared.Integration.Models;

public class PagedList<T>
{
	public ICollection<T> Items { get; set; }
	public int PageNumber { get; set; }
	public int TotalPages { get; set; }
	public int TotalCount { get; set; }

	public bool HasPreviousPage => PageNumber > 1;
	public bool HasNextPage => PageNumber < TotalPages;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	public PagedList()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	{

	}

	public PagedList(int pageNumber, int totalCount, int totalPages, ICollection<T> items)
	{
		Items = items;
		PageNumber = pageNumber;
		TotalCount = totalCount;
		TotalPages = totalPages;
	}

	public PagedList(ICollection<T> items, int totalCount, int pageNumber, int pageSize)
	{
		PageNumber = pageNumber;
		TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
		TotalCount = totalCount;
		Items = items;
	}
}