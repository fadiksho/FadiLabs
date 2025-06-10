using Fadi.Result;
using Fadi.Result.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Modules.Blog.Integration.Post;
using Moq;
using Shared.Features.Behaviours;
using Shared.Features.Services;
using System.Security.Claims;

namespace Shared.Features.Test.Behaviours;

public class AuthorizationBehaviourTest
{
	private readonly Mock<ICurrentUser> _currentUser;
	private readonly Mock<IAuthorizationService> _authorizationService;
	private readonly Mock<ClaimsPrincipal> _claimsPrincipal;
	private readonly Mock<ClaimsIdentity> _claimsIdentity;
	public AuthorizationBehaviourTest()
	{
		_currentUser = new Mock<ICurrentUser>();
		_authorizationService = new Mock<IAuthorizationService>();
		_claimsPrincipal = new Mock<ClaimsPrincipal>();
		_claimsIdentity = new Mock<ClaimsIdentity>();
	}

	[Fact]
	public async Task Handle_NoAuthorizeAttributes_ContinuesPipeline()
	{
		// Arrange
		var authorizationBehaviour =
			new AuthorizationBehaviour<GetPostBySlug, Result<GetPostBySlugResponse>>(_currentUser.Object, _authorizationService.Object);
		var requestDelegateHandlerMock = new Mock<RequestHandlerDelegate<Result<GetPostBySlugResponse>>>();

		var request = new GetPostBySlug("slug");

		// Act
		var responseDelegate = await authorizationBehaviour.Handle(request, requestDelegateHandlerMock.Object, default);

		// Assert
		_currentUser.Verify(i => i.GetUser(), Times.Never);
		_authorizationService.Verify(i => i.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()), Times.Never);
		requestDelegateHandlerMock.Verify(i => i.Invoke(), Times.Once);
	}

	[Fact]
	public async Task Handle_UserNotAuthenticated_ReturnsUnauthenticatedError()
	{
		// Arrange
		var authorizationBehaviour =
			new AuthorizationBehaviour<CreatePost, Result<CreatePostResponse>>(_currentUser.Object, _authorizationService.Object);
		var requestDelegateHandlerMock = new Mock<RequestHandlerDelegate<Result<CreatePostResponse>>>();

		var request = new CreatePost();

		// Act
		var responseDelegate = await authorizationBehaviour.Handle(request, requestDelegateHandlerMock.Object, default);

		// Assert
		_currentUser.Verify(i => i.GetUser(), Times.Once);
		_authorizationService.Verify(i => i.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()), Times.Never);

		Assert.False(responseDelegate.IsSuccess);
		Assert.IsType<UnauthentectedError>(responseDelegate.Error);
		requestDelegateHandlerMock.Verify(i => i.Invoke(), Times.Never);
	}

	[Fact]
	public async Task Handle_AuthorizationFails_ReturnsUnauthorizedError()
	{
		// Arrange
		_claimsIdentity.Setup(x => x.IsAuthenticated).Returns(true);
		_claimsPrincipal.Setup(x => x.Identity).Returns(_claimsIdentity.Object);
		_currentUser.Setup(i => i.GetUser()).Returns(Task.FromResult(_claimsPrincipal.Object));

		_authorizationService.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
			.ReturnsAsync(AuthorizationResult.Failed());

		var authorizationBehaviour =
			new AuthorizationBehaviour<CreatePost, Result<CreatePostResponse>>(_currentUser.Object, _authorizationService.Object);
		var requestDelegateHandlerMock = new Mock<RequestHandlerDelegate<Result<CreatePostResponse>>>();
		var request = new CreatePost();

		// Act
		var responseDelegate = await authorizationBehaviour.Handle(request, requestDelegateHandlerMock.Object, default);

		// Assert
		_currentUser.Verify(i => i.GetUser(), Times.Once);
		_authorizationService.Verify(i => i.AuthorizeAsync(_claimsPrincipal.Object, It.IsAny<object>(), It.IsAny<string>()), Times.Once);

		Assert.False(responseDelegate.IsSuccess);
		Assert.IsType<UnauthorizedError>(responseDelegate.Error);
		requestDelegateHandlerMock.Verify(i => i.Invoke(), Times.Never);
	}

	[Fact]
	public async Task Handle_AuthorizationSucceeds_ContinuesPipeline()
	{
		// Arrange
		_claimsIdentity.Setup(x => x.IsAuthenticated).Returns(true);
		_claimsPrincipal.Setup(x => x.Identity).Returns(_claimsIdentity.Object);
		_currentUser.Setup(i => i.GetUser()).Returns(Task.FromResult(_claimsPrincipal.Object));

		_authorizationService.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>()))
			.ReturnsAsync(AuthorizationResult.Success());

		var authorizationBehaviour =
			new AuthorizationBehaviour<CreatePost, Result<CreatePostResponse>>(_currentUser.Object, _authorizationService.Object);
		var requestDelegateHandlerMock = new Mock<RequestHandlerDelegate<Result<CreatePostResponse>>>();
		var request = new CreatePost();

		// Act
		var responseDelegate = await authorizationBehaviour.Handle(request, requestDelegateHandlerMock.Object, default);

		// Assert
		_currentUser.Verify(i => i.GetUser(), Times.Once);
		_authorizationService.Verify(i => i.AuthorizeAsync(_claimsPrincipal.Object, It.IsAny<object>(), It.IsAny<string>()), Times.Once);

		requestDelegateHandlerMock.Verify(i => i.Invoke(), Times.Once);
	}
}

