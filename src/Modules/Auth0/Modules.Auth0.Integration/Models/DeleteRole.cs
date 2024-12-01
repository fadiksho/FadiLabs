namespace Modules.Auth0.Integration.Models;
public record DeleteRole(string RoleId) : IRequest<Result>;