using Fadi.Result.Errors;
using Fadi.Result.Serialization;
using MediatR;
using Shared.Integration.Services;
using Shared.Integration.Services.Implementaions;

namespace Fadi.Result.Tests;
public static class ResultEnvelopSerialization
{
	record ModelTestRequest() : IRequest<Result<ModelTestResponse>>;
	record ModelTestResponse();

	static IEnvelopMessageHandler EnvelopMessageHandler
		= new DefaultEnvelopMessageHandler([new DefaultResultErrorPolymorphicResolver()]);
	public class TestWrapUnwrap
	{
		[Test]
		public void ValidationErrorResult()
		{
			var validationError = new ValidationError { Message = "Dummy message", Identifier = "Test", Severity = ValidationSeverity.Error };
			var errorResult = new ValidationErrorResult([validationError]);
			var unsuccessful = Result<ModelTestResponse>.FromError(errorResult);

			var wrap = EnvelopMessageHandler.Wrap(unsuccessful);

			var unWrapBody = EnvelopMessageHandler.UnwrapBody<Result<ModelTestResponse>>(wrap);

			Assert.That(wrap, Is.Not.Null);
			Assert.That(unWrapBody.Error, Is.InstanceOf<ValidationErrorResult>());
		}

		[Test]
		public void ErrorResult()
		{
			var error = new GenericError("Dummy message");
			var unsuccessful = Result<ModelTestResponse>.FromError(error);

			var wrap = EnvelopMessageHandler.Wrap(unsuccessful);

			var unWrapBody = EnvelopMessageHandler.UnwrapBody<Result<ModelTestResponse>>(wrap);

			Assert.That(wrap, Is.Not.Null);
			Assert.That(unWrapBody.Error, Is.InstanceOf<GenericError>());
		}
	}


}
