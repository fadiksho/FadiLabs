using Fadi.Result.Serialization;
using Shared.Integration.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Shared.Integration.Services.Implementaions;

public class DefaultEnvelopMessageHandler : IEnvelopMessageHandler
{
	public JsonSerializerOptions JsonOptions { get; }

	public DefaultEnvelopMessageHandler(IEnumerable<IResultErrorPolymorphicResolver> resultErrorPolymorphics)
	{
		var infoResolver = new DefaultJsonTypeInfoResolver();

		foreach (var resolver in resultErrorPolymorphics)
		{
			infoResolver.Modifiers.Add(resolver.ResolveDerivedType);
		}

		JsonOptions = new()
		{
			ReferenceHandler = ReferenceHandler.Preserve,
			TypeInfoResolver = infoResolver,
			IncludeFields = true,
		};
	}

	public object Unwrap(EnvelopeMessage message)
	{

		var type = Type.GetType(message.TypeName, true)!;
		return JsonSerializer.Deserialize(message.Body, type, JsonOptions)!;
	}

	public T UnwrapBody<T>(EnvelopeMessage message)
	{
		return JsonSerializer.Deserialize<T>(message.Body, JsonOptions)!;
	}

	public EnvelopeMessage Wrap(object message)
	{
		return new EnvelopeMessage
		{
			Body = JsonSerializer.Serialize(message, message.GetType(), JsonOptions),
			TypeName = message.GetType().FullName + ", " + message.GetType().Assembly.GetName().Name
		};
	}
}
