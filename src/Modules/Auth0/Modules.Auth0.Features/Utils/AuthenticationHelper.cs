using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shared.Integration.Services;

namespace Modules.Auth0.Features.Utils;
public static class AuthenticationHelper
{
	/// <summary>
	/// Writes a JSON response to the HTTP context with the specified status code and error message.
	/// </summary>
	/// <param name="context">The HTTP context to write the response to.</param>
	/// <param name="statusCode">The HTTP status code to set in the response.</param>
	/// <param name="error">The error message to include in the response.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public static async Task WriteJsonResponse(HttpContext context, int statusCode, IResultError error)
	{
		context.Response.StatusCode = statusCode;
		context.Response.ContentType = "application/json";

		var envelopMessage = context.RequestServices.GetRequiredService<IEnvelopMessageHandler>();
		var wrappedResponse = envelopMessage.Wrap(Result.FromError(error));
		var jsonStringResponse = JsonSerializer.Serialize(wrappedResponse, envelopMessage.JsonOptions);

		await context.Response.WriteAsync(jsonStringResponse);
	}
}

