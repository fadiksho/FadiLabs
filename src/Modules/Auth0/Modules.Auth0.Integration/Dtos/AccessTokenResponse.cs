namespace Modules.Auth0.Integration.Dtos;

public record AccessTokenResponse
{
	//[JsonPropertyName("access_token")]
	public string? AccessToken { get; set; }
	//[JsonPropertyName("scope")]
	public string? Scope { get; set; }
	//[JsonPropertyName("expire_in")]
	public int ExpiresIn { get; set; }
	//[JsonPropertyName("token_type")]
	public string? TokenType { get; set; }
}