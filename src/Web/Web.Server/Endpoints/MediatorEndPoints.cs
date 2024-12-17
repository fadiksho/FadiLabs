using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Shared.Integration.Models;
using Shared.Integration.Services;

namespace Web.Server.Endpoints;

internal static class MediatorEndPoints
{
	public static void MapMediatorEndPoints(this IEndpointRouteBuilder endpoints)
	{
		var mediatorEndPoints = endpoints.MapGroup("/api");

		mediatorEndPoints.MapPost("/send", async (
			HttpContext context,
			EnvelopeMessage message,
			[FromServices] IMediator mediator,
			[FromServices] IEnvelopMessageHandler envelopMessage,
			[FromServices] IAuthenticationService authService) =>
		{
			var unwrappedRequest = envelopMessage.Unwrap(message);
			var response = await mediator.Send(unwrappedRequest) ?? throw new InvalidOperationException();
			var wrappedResponse = envelopMessage.Wrap(response);

			await context.Response.WriteAsJsonAsync(wrappedResponse, envelopMessage.JsonOptions);
		});
	}
}
