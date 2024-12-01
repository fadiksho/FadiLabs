using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Components.Components.Forms;
public class FluentValidationValidator : ComponentBase
{
	private IValidator? _validator;

	private ValidationMessageStore? _validationMessageStore;

	[CascadingParameter]
	private EditContext? EditContext { get; set; }

	[Parameter]
	public Type? ValidatorType { get; set; }

	[Inject]
	private IServiceProvider ServiceProvider { get; set; } = default!;

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		// Keep a reference to the original values so we can check if they have changed
		EditContext? previousEditContext = EditContext;
		Type? previousValidatorType = ValidatorType;

		await base.SetParametersAsync(parameters);

		if (EditContext == null)
		{
			throw new NullReferenceException($"{nameof(FluentValidationValidator)} must be placed within an {nameof(EditForm)}");
		}

		if (ValidatorType == null)
		{
			throw new NullReferenceException($"{nameof(ValidatorType)} must be specified.");
		}

		if (!typeof(IValidator).IsAssignableFrom(ValidatorType))
		{
			throw new ArgumentException($"{ValidatorType.Name} must implement {typeof(IValidator).FullName}");
		}

		if (ValidatorType != previousValidatorType)
		{
			_validator = (IValidator)ServiceProvider.GetRequiredService(ValidatorType);
		}

		// If the EditForm.Model changes then we get a new EditContext
		if (EditContext != previousEditContext)
		{
			_validationMessageStore = new ValidationMessageStore(EditContext);
			EditContext.OnValidationRequested += ValidationRequested;
			EditContext.OnFieldChanged += FieldChanged;
		}
	}

	async void ValidationRequested(object? sender, ValidationRequestedEventArgs args)
	{
		ArgumentNullException.ThrowIfNull(_validator);

		if (EditContext != null)
		{
			_validationMessageStore?.Clear();
			var validationContext = new ValidationContext<object>(EditContext.Model);
			var result = await _validator.ValidateAsync(validationContext);
			AddValidationResult(EditContext.Model, result);
		}
	}

	async void FieldChanged(object? sender, FieldChangedEventArgs args)
	{
		ArgumentNullException.ThrowIfNull(_validator);

		FieldIdentifier fieldIdentifier = args.FieldIdentifier;
		_validationMessageStore?.Clear(fieldIdentifier);

		var propertiesToValidate = new string[] { fieldIdentifier.FieldName };
		var fluentValidationContext =
				new ValidationContext<object>(
						instanceToValidate: fieldIdentifier.Model,
						propertyChain: new FluentValidation.Internal.PropertyChain(),
						validatorSelector: new FluentValidation.Internal.MemberNameValidatorSelector(propertiesToValidate)
				);

		var result = await _validator.ValidateAsync(fluentValidationContext);

		AddValidationResult(fieldIdentifier.Model, result);
	}

	private void AddValidationResult(object model, ValidationResult validationResult)
	{
		if (EditContext is not null)
		{
			foreach (ValidationFailure error in validationResult.Errors)
			{
				var fieldIdentifier = new FieldIdentifier(model, error.PropertyName);

				_validationMessageStore?.Add(fieldIdentifier, error.ErrorMessage);
			}
			EditContext.NotifyValidationStateChanged();
		}
	}
}