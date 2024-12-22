using System.Data.Common;

namespace Modules.User.Features.Test;
public interface ITestDatabase
{
	Task InitialiseAsync();

	DbConnection GetConnection();

	string GetConnectionString();

	Task ResetAsync();

	Task DisposeAsync();
}
