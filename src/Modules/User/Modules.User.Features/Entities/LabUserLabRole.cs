namespace Modules.User.Features.Entities;

public class LabUserLabRole
{
	public Guid LabUserId { get; set; }
	public LabUser? LabUser { get; set; }

	public Guid LabRoleId { get; set; }
	public LabRole? LabRole { get; set; }
}
