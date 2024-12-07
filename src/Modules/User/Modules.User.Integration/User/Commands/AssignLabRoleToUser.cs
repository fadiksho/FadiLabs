namespace Modules.User.Integration.User.Commands;

public record AssignLabRoleToUser : IRequest<Result>
{
	public Guid LabRoleId { get; set; }
	public Guid LabUserId { get; set; }
}
