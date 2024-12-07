namespace Modules.User.Integration.User.Commands;

public record DeleteLabRole(Guid LabRoleId) : IRequest<Result>;