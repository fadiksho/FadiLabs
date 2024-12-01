namespace Modules.Auth0.Integration.Models;
public record DeleteUser(string UserId) : IRequest<Result>;