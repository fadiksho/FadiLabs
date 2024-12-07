using Modules.Shared.Integration.Authorization;

namespace Modules.Auth0.Integration.Models;

[LabAuthorize(LabsPermissions.ConfigureSite)]
public record DeleteUser(string UserId) : IRequest<Result>;