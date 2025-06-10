using Fadi.Result.Errors;
using FluentValidation;
using MediatR;
using Modules.Shared.Integration.Extensions;

namespace Shared.Features.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
		where TRequest : notnull, IRequest<TResponse>
		where TResponse : Fadi.Result.IResult
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (validators.Any())
		{
			var context = new ValidationContext<TRequest>(request);

			var validationResults = await Task.WhenAll(
					validators.Select(v =>
							v.ValidateAsync(context, cancellationToken)));

			var failures = validationResults
				.Where(r => r.Errors.Count > 0)
				.SelectMany(r => r.Errors)
				.Select(x =>
					new ValidationError
					{
						Message = x.ErrorMessage,
						Severity = (ValidationSeverity)x.Severity,
						Identifier = x.PropertyName
					})
				.ToArray();

			if (failures.Length > 0)
			{
				return next.FromError(request, new ValidationErrorResult(failures));
			}
		}
		return await next();
	}
}
