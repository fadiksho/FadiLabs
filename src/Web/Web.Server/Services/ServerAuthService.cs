using Shared.Components.Services;

namespace Web.Server.Services;

public class ServerAuthService : IAuthService
{
	public Task AuthenticateAsync()
	{
		throw new NotImplementedException();
	}

	public Task DeAuthenticateAsync()
	{
		throw new NotImplementedException();
	}
}
