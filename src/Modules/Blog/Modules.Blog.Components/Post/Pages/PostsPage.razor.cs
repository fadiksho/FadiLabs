using Fadi.Result;
using Microsoft.AspNetCore.Components;
using Modules.Blog.Integration.Post;
using Modules.Shared.Integration.Models;
using Shared.Components.Services;
using System.Diagnostics;

namespace Modules.Blog.Components.Post.Pages;
public partial class PostsPage(IUIBus bus, NavigationManager navigation)
{
	[Parameter]
	[SupplyParameterFromQuery(Name = "page")]
	public int Page { get; set; }

	[Parameter]
	[SupplyParameterFromQuery(Name = "tag")]
	public string Tag { get; set; } = string.Empty;

	[Parameter]
	[SupplyParameterFromQuery(Name = "q")]
	public string SearchQuery { get; set; } = string.Empty;

	private Result<PagedList<GetPostsResponse>> result = new();

	protected override async Task OnParametersSetAsync()
	{
		Debug.WriteLine($"{nameof(OnParametersSetAsync)}");
		if (Page <= 0)
		{
			Page = 1;
		}

		result = await bus.Send(new GetPosts
		{
			PageNumber = Page,
			Tag = Tag,
			Search = SearchQuery
		});
	}

	private string GetClearFilterLink(string filterKey)
	{
		var clearFilterLink = navigation.GetUriWithQueryParameters(
			navigation.GetUriWithQueryParameter(filterKey, (string?)null),
			new Dictionary<string, object?>
			{
				["page"] = null,
			}
		);

		return clearFilterLink;
	}
}
