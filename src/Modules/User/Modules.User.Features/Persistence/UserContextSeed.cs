using Microsoft.EntityFrameworkCore;
using Modules.Shared.Integration.Authorization;
using Modules.User.Features.Entities;
using Shared.Features.Persistence;

namespace Modules.User.Features.Persistence;

public class UserContextSeed : IContextSeed
{
	public static void Seed(DbContext context, bool _)
	{
		if (context.Set<LabRole>().Any())
			return;

		var roles = new List<LabRole>
		{
			new() {
				Name = "admin",
				Description = "default admin role.",
				LabsPermissions = LabsPermissions.All
			},
			new()
			{
				Name = "default lab role",
				Description = "Auto assigned for new account.",
				LabsPermissions = LabsPermissions.None
			}
		};

		context.Set<LabRole>().AddRange(roles);

		context.SaveChanges();
	}
}
