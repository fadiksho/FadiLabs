namespace Modules.Shared.Integration.Authorization;

[Flags]
public enum Permissions
{
	None = 0,

	ConfigureAccessControl = 1,

	BlogOwner = 2,

	All = ~None
}
