using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Modules.User.Features.Persistence;
using Shared.Features.Configuration;
using Shared.Features.Persistence.Interceptors;
using System.Data;

namespace Modules.User.Features.Test;
public class SqliteTestDatabase
{
	private readonly string _connectionString;
	private readonly SqliteConnection _connection;

	private UserContext? _context;

	public SqliteTestDatabase()
	{
		_connectionString = "DataSource=:memory:";
		_connection = new SqliteConnection(_connectionString);
	}

	public async Task InitialiseAsync()
	{
		if (_connection.State == ConnectionState.Open)
		{
			await _connection.CloseAsync();
		}

		await _connection.OpenAsync();

		var options = new DbContextOptionsBuilder<UserContext>()
				.UseSqlite(_connection)
				.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning))
				.AddInterceptors(
					new AuditableEntityInterceptor(TimeProvider.System))
				.Options;

		var persistenceOptions = Options.Create(new PersistenceConfiguration
		{
			ConnectionString = _connectionString
		});

		_context = new UserContext(options, persistenceOptions);

		_context.Database.EnsureCreated();
	}

	public UserContext GetContext()
	{
		return _context
			?? throw new ArgumentNullException($"Call {nameof(InitialiseAsync)} first.");
	}

	public async Task DisposeAsync()
	{
		_context?.Database.EnsureDeleted();
		await _connection.DisposeAsync();
	}
}
