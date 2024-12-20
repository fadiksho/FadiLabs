using Fadi.Result;
using Microsoft.AspNetCore.Components;
using Modules.Blog.Integration.Post;
using Shared.Components.Services;

namespace Modules.Blog.Components.InteractivePages;

public partial class EditPostPage(IServiceExecutor<IUIBus> serviceExecutor)
{
	[Parameter]
	public Guid Id { get; set; }

	private Result<UpdatePost> _getToUpdatePostResult;

	protected override async Task OnInitializedAsync()
	{
		_getToUpdatePostResult = await serviceExecutor
			.ExecuteAsync(nameof(GetToUpdatePost), x => x.Send(new GetToUpdatePost(Id)), true);
	}
}
