using Fadi.Result;
using Microsoft.AspNetCore.Components;
using Modules.Blog.Integration.Post;
using Shared.Components.Services;

namespace Modules.Blog.Components.Post.Pages;
public partial class PostDetailsPage(IUIBus bus)
{
	[Parameter]
	public string? Slug { get; set; }

	private Result<GetPostBySlugResponse> result;

	protected override async Task OnInitializedAsync()
	{
		if (string.IsNullOrEmpty(Slug))
		{
			throw new ArgumentNullException(nameof(Slug));
		}

		result = await bus.Send(new GetPostBySlug(Slug));
	}
}
