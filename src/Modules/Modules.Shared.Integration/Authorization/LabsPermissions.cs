namespace Modules.Shared.Integration.Authorization;

[Flags]
public enum LabsPermissions
{
	None = 0,

	ConfigureSite = 1,

	BlogOwner = 2,

	All = ~None
}
