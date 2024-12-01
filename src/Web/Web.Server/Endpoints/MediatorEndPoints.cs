using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Shared.Integration.Models;
using Shared.Integration.Services;
using System.Text.Json;

namespace Web.Server.Endpoints;

internal static class MediatorEndPoints
{
	public static void MapMediatorEndPoints(this IEndpointRouteBuilder endpoints)
	{
		var mediatorEndPoints = endpoints.MapGroup("/api");

		mediatorEndPoints.MapPost("/send", async (
			EnvelopeMessage message,
			[FromServices] IMediator mediator,
			[FromServices] IEnvelopMessageHandler envelopMessage,
			[FromServices] IAuthenticationService authService) =>
		{
			var unwrappedRequest = envelopMessage.Unwrap(message);
			var response = await mediator.Send(unwrappedRequest) ?? throw new InvalidOperationException();
			var wrappedResponse = envelopMessage.Wrap(response);

			return JsonSerializer.Serialize(wrappedResponse, envelopMessage.JsonOptions);
		});
	}
}
