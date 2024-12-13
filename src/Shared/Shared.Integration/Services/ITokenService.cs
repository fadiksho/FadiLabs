namespace Shared.Integration.Services;

public interface ITokenService
{
	Task<GetTokensResponse> GetTokens();
}

public record GetTokensResponse
{
	public string? IdToken { get; set; }
	public string? AccessToken { get; set; }
	public string? RefreshToken { get; set; }
}