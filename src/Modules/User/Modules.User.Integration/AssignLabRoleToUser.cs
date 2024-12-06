namespace Modules.User.Integration;

public record AssignLabRoleToUser : IRequest<Result>
{
	public Guid LabRoleId { get; set; }
	public Guid LabUserId { get; set; }
}
