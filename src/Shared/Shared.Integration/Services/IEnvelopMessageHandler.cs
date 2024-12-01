using Shared.Integration.Models;
using System.Text.Json;

namespace Shared.Integration.Services;

public interface IEnvelopMessageHandler
{
  JsonSerializerOptions JsonOptions { get; }

  EnvelopeMessage Wrap(object message);

  object Unwrap(EnvelopeMessage message);

  T UnwrapBody<T>(EnvelopeMessage message);
}
