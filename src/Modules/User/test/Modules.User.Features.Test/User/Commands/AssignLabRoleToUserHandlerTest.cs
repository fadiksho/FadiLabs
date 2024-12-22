using Fadi.Result.Errors;
using Microsoft.EntityFrameworkCore;
using Modules.User.Features.Entities;
using Modules.User.Features.User.Commands;
using Modules.User.Integration.User.Commands;
using Modules.User.Integration.User.ResultErrors;

namespace Modules.User.Features.Test.User.Commands;

public class AssignLabRoleToUserHandlerTest : IAsyncLifetime
{
	private readonly List<LabRole> _dummyLabRoles =
	[
		new()
		{
			Id = Guid.NewGuid(),
			Name = "role1",
			Description = "description1",
			AutoAssign = true,
		},
		new()
		{
			Id = Guid.NewGuid(),
			Name = "role2",
			Description = "description2",
			AutoAssign = false,
		}
	];

	private readonly List<LabUser> _dummyLabUsers =
	[
		new()
		{
			Id = Guid.NewGuid(),
			Auth0UserId = "auth01",
			Email = "test@test.com"
		}
	];

	private readonly SqliteTestDatabase _sqliteTestDatabase;

	public AssignLabRoleToUserHandlerTest()
	{
		_sqliteTestDatabase = new SqliteTestDatabase();
	}

	[Fact]
	public async Task Handle_UserOrRoleNotFound_ReturnsNotFoundError()
	{
		// Arrange
		var userContext = _sqliteTestDatabase.GetContext();
		await SeedAsync(userContext);

		var assignLabRoleToUser1 = new AssignLabRoleToUser
		{
			LabUserId = _dummyLabUsers[0].Id,
			LabRoleId = Guid.NewGuid(),
		};
		var assignLabRoleToUser2 = new AssignLabRoleToUser
		{
			LabUserId = Guid.NewGuid(),
			LabRoleId = _dummyLabRoles[0].Id
		};
		var handler = new AssignLabRoleToUserHandler(userContext);

		// Act
		var result = await handler.Handle(assignLabRoleToUser1, default);
		var result2 = await handler.Handle(assignLabRoleToUser2, default);

		// Assert 1
		Assert.True(result.IsFailed);
		Assert.IsType<NotFoundError>(result.Error);

		// Assert 2
		Assert.True(result2.IsFailed);
		Assert.IsType<NotFoundError>(result2.Error);
	}

	[Fact]
	public async Task Handle_RoleAutoAssign_ReturnsInvalidRoleAssignmentError()
	{
		// Arrange
		var userContext = _sqliteTestDatabase.GetContext();
		await SeedAsync(userContext);

		var assignLabRoleToUser = new AssignLabRoleToUser
		{
			LabUserId = _dummyLabUsers[0].Id,
			LabRoleId = _dummyLabRoles[0].Id
		};
		var handler = new AssignLabRoleToUserHandler(userContext);

		// Act
		var result = await handler.Handle(assignLabRoleToUser, default);

		// Assert 1
		Assert.True(result.IsFailed);
		Assert.IsType<InvalidRoleAssignmentError>(result.Error);
	}

	[Fact]
	public async Task Handle_ValidUserAndRole_ReturnsSuccess()
	{
		// Arrange
		var userContext = _sqliteTestDatabase.GetContext();
		await SeedAsync(userContext);

		AssignLabRoleToUser assignLabRoleToUser = new()
		{
			LabUserId = _dummyLabUsers[0].Id,
			LabRoleId = _dummyLabRoles[1].Id
		};
		var handler = new AssignLabRoleToUserHandler(userContext);

		// Act
		var result = await handler.Handle(assignLabRoleToUser, default);

		// Assert
		Assert.True(result.IsSuccess);
		var user = userContext.LabUsers
			.First(x => x.Id == assignLabRoleToUser.LabUserId);
		Assert.Single(user.LabRoles);
	}

	public async Task InitializeAsync()
	{
		await _sqliteTestDatabase.InitialiseAsync();
	}

	public async Task DisposeAsync()
	{
		await _sqliteTestDatabase.DisposeAsync();
	}

	private async Task SeedAsync(DbContext context)
	{
		context.AddRange(_dummyLabUsers);
		context.AddRange(_dummyLabRoles);

		await context.SaveChangesAsync();
	}
}
