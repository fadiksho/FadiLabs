using Auth0PagedList = Auth0.ManagementApi.Paging;

namespace Modules.Auth0.Features.Utils;

internal static class Auth0ManagmentApiHelper
{
	public static PagedList<T> ToPagedList<T>(ICollection<T> items, Auth0PagedList.PagingInformation pagingInfo)
	{
		return new PagedList<T>
		{
			Items = items,
			PageNumber = pagingInfo.Start / pagingInfo.Limit + 1,
			TotalPages = (int)Math.Ceiling((double)pagingInfo.Total / pagingInfo.Limit),
			TotalCount = pagingInfo.Total
		};
	}
}
