namespace Modules.User.Features.Entities;
public class LinkedAccount
{
	public Guid LabUserId { get; set; }
	public required string ProviderKey { get; set; }
	public required string Provider { get; set; }
	public required string Connection { get; set; }
	public bool IsSocial { get; set; }
}
