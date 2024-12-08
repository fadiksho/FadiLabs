namespace Modules.User.Integration.User.ResultErrors;

public sealed record InvalidRoleAssignmentError(string Message) : ResultError(Message);