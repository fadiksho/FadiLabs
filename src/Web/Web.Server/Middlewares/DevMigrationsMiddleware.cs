using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Web.Server.Middlewares;

public class DevMigrationsMiddleware(
	RequestDelegate next,
	ILogger<DevMigrationsMiddleware> logger)
{
	private readonly PathString _migrationPath = new("/apply-migration");

	public virtual Task Invoke(HttpContext context)
	{
		ArgumentNullException.ThrowIfNull(context);

		if (context.Request.Path.Equals(_migrationPath))
		{
			return ApplyDevMigrationAsync(context);
		}

		return next(context);
	}

	async Task ApplyDevMigrationAsync(HttpContext context)
	{
		var registeredContexts = context.RequestServices.GetServices<DbContextOptions>()
						.Select(o => o.ContextType)
						.Distinct();

		if (registeredContexts == null)
			return;

		foreach (var registeredContext in registeredContexts)
		{
			var contextDetail = await context.GetContextDetailsAsync(registeredContext, logger);
			if (contextDetail == null)
				continue;

			if (contextDetail.PendingMigrations.Any())
			{
				try
				{
					logger.LogInformation("Applying migrations for context '{ContextTypeName}'", contextDetail.DatabaseName);

					await contextDetail.DatabaseContext.Database.MigrateAsync();

					logger.LogInformation("Migrations successfully applied for context '{ContextTypeName}'", contextDetail.DatabaseName);
				}
				catch (Exception ex)
				{
					logger.LogError("An error occurred while applying the migrations for '{ContextTypeName}'", contextDetail.DatabaseName);

					throw new InvalidOperationException(ex.Message, ex);
				}
			}
		}
	}
}

internal static class DevMigrationMiddlewareExtensions
{
	internal static async ValueTask<DatabaseContextDetails?> GetContextDetailsAsync(this HttpContext httpContext, Type dbcontextType, ILogger logger)
	{
		var dbContext = (DbContext?)httpContext.RequestServices.GetService(dbcontextType);

		if (dbContext == null)
		{
			logger.LogError("The context type '{ContextTypeName}' was not found in services. This usually means the context was not registered in services during startup. You probably want to call AddScoped<>() inside the UseServices(...) call in your application startup code. Skipping display of the database error page.", dbcontextType.FullName!);
			return null;
		}

		if (dbContext.GetService<IDatabaseCreator>() is not IRelationalDatabaseCreator relationalDatabaseCreator)
		{
			logger.LogDebug("The target data store is not a relational database. Skipping the database error page.");
			return null;
		}

		var databaseExists = await relationalDatabaseCreator.ExistsAsync();

		if (databaseExists)
		{
			databaseExists = await relationalDatabaseCreator.HasTablesAsync();
		}

		var migrationsAssembly = dbContext.GetService<IMigrationsAssembly>();
		var modelDiffer = dbContext.GetService<IMigrationsModelDiffer>();

		var snapshotModel = migrationsAssembly.ModelSnapshot?.Model;

		if (snapshotModel is IMutableModel mutableModel)
		{
			snapshotModel = mutableModel.FinalizeModel();
		}

		if (snapshotModel != null)
		{
			snapshotModel = dbContext.GetService<IModelRuntimeInitializer>().Initialize(snapshotModel);
		}

		// HasDifferences will return true if there is no model snapshot, but if there is an existing database
		// and no model snapshot then we don't want to show the error page since they are most likely targeting
		// and existing database and have just misconfigured their model
		var pendingModelChanges = (!databaseExists || migrationsAssembly.ModelSnapshot != null)
						&& modelDiffer.HasDifferences(
								snapshotModel?.GetRelationalModel(),
								dbContext.GetService<IDesignTimeModel>().Model.GetRelationalModel());

		var pendingMigrations = databaseExists
						? await dbContext.Database.GetPendingMigrationsAsync()
						: dbContext.Database.GetMigrations();

		var contextDetails = new DatabaseContextDetails
		{
			DatabaseName = dbcontextType.Name,
			DatabaseContext = dbContext,
			DatabaseExists = databaseExists,
			PendingModelChanges = pendingModelChanges,
			PendingMigrations = pendingMigrations
		};

		return contextDetails;
	}
}

internal sealed class DatabaseContextDetails
{
	public required DbContext DatabaseContext { get; init; }
	public required string DatabaseName { get; init; }
	public required bool DatabaseExists { get; init; }
	public required bool PendingModelChanges { get; init; }
	public required IEnumerable<string> PendingMigrations { get; init; }
}
