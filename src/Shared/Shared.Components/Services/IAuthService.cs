namespace Shared.Components.Services;
public interface IAuthService
{
	Task AuthenticateAsync();
	Task DeAuthenticateAsync();
}
