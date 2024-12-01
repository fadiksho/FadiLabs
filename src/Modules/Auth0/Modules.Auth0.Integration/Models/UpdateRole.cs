namespace Modules.Auth0.Integration.Models;
public record UpdateRole : IRequest<Result>
{
	public required string RoleId { get; set; }
	public required string Name { get; set; }
	public required string Description { get; set; }
}
