using Fadi.Result;
using Microsoft.AspNetCore.Components;
using Modules.Blog.Integration.Post;
using Shared.Components.Services;

namespace Modules.Blog.Components.StaticPages;
public partial class PostDetailsPage(IUIBus bus)
{
	[Parameter]
	public string? Slug { get; set; }

	private Result<GetPostBySlugResponse> _result;

	protected override async Task OnInitializedAsync()
	{
		if (string.IsNullOrEmpty(Slug))
		{
			throw new ArgumentNullException(nameof(Slug));
		}

		_result = await bus.Send(new GetPostBySlug(Slug));
	}
}
