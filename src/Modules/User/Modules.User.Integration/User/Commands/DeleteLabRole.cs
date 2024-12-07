namespace Modules.User.Integration.User.Commands;

[LabAuthorize(LabsPermissions.ConfigureSite)]
public record DeleteLabRole(Guid LabRoleId) : IRequest<Result>;