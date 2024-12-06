namespace Modules.User.Integration;

public record DeleteLabRole(Guid LabRoleId) : IRequest<Result>;