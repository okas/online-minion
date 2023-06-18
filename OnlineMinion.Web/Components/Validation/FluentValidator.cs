using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace OnlineMinion.Web.Components.Validation;

public class FluentValidator : ComponentBase
{
    private ValidationMessageStore _messageStore = null!;

    private Type _modelType = null!; // TODO: Use generic model type param  instead of reflection;

    private List<IValidator> _validatorsCache = new();

    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = null!;

    [CascadingParameter]
    public EditContext CurrentEditContext { get; set; } = null!;

    [CascadingParameter]
    public CancellationToken CT { get; set; }

    [Parameter]
    public IValidator? Validator { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        CheckEditorContext();

        _messageStore = new(CurrentEditContext);
        _modelType = CurrentEditContext.Model.GetType();

        SetUpValidators();

        CurrentEditContext.OnValidationRequested += ModelValidationHandler;
        CurrentEditContext.OnFieldChanged += FieldValidationHandler;
    }

    private void CheckEditorContext()
    {
        if (CurrentEditContext is null)
        {
            throw new InvalidOperationException(
                $"{nameof(FluentValidator)} requires a cascading " +
                $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(FluentValidator)} " +
                $"inside an {nameof(EditForm)}."
            );
        }
    }

    private void SetUpValidators()
    {
        if (Validator is not null)
        {
            _validatorsCache.Add(Validator);
        }
        else
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(_modelType);
            var validators = (IEnumerable<IValidator>)ServiceProvider.GetServices(validatorType);
            _validatorsCache.AddRange(validators);

            if (_validatorsCache.Count == 0)
            {
                throw new InvalidOperationException(
                    $"{nameof(FluentValidator)} expects that validators are registered in DI container before usage. "
                    + $"No validators for type of model '{_modelType}'  where found."
                );
            }
        }
    }

    private async void ModelValidationHandler(object? _, ValidationRequestedEventArgs __)
    {
        _messageStore.Clear();
        await ValidateByStrategyAsync(strategy => strategy.IncludeAllRuleSets());
        CurrentEditContext.NotifyValidationStateChanged();
    }

    private async void FieldValidationHandler(object? _, FieldChangedEventArgs evt)
    {
        //TODO: Needs throttling in case any validator makes async calls outside of browser!
        _messageStore.Clear(evt.FieldIdentifier);
        await ValidateByStrategyAsync(strategy => strategy.IncludeProperties(evt.FieldIdentifier.FieldName));
        CurrentEditContext.NotifyValidationStateChanged();
    }


    /// <summary>
    ///     Validates this <see cref="EditContext" />.
    /// </summary>
    private async ValueTask ValidateByStrategyAsync(Action<ValidationStrategy<object>> strategy)
    {
        var validationFailures = await GetValidationFailures(strategy);

        validationFailures.ForEach(
            failure => _messageStore.Add(
                CurrentEditContext.Field(failure.PropertyName),
                failure.ErrorMessage
            )
        );
    }

    private async ValueTask<List<ValidationFailure>> GetValidationFailures(Action<ValidationStrategy<object>> strategy)
    {
        var tasks = _validatorsCache.Select(
            validator => validator.ValidateAsync(CreateValidationContext(strategy), CT)
        );

        var validationResults = await Task.WhenAll(tasks);

        return validationResults
            .SelectMany(vr => vr.Errors)
            .Where(failure => failure is not null)
            .ToList();
    }

    private ValidationContext<object> CreateValidationContext(Action<ValidationStrategy<object>> strategy) =>
        ValidationContext<object>.CreateWithOptions(CurrentEditContext.Model, strategy);
}
