using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Common.Validation;

namespace OnlineMinion.SPA.Blazor.Shared.Forms;

/// <summary>
///     Validates model instance using FluentValidation's <see cref="FluentValidation.IValidator" /> validator instances
///     obtained  from DI container or explicitly provided from parameter <see cref="Validators" />.
/// </summary>
/// <remarks>
///     If the <see cref="EditForm" /> is known to validate model using validators of
///     <see cref="IAsyncUniqueValidator{TModel}" />, then only use <see cref="EditForm.OnSubmit" /> handler and perform
///     validation from this handler manually.<br />
///     This is because as of ".NET 8 preview 5" async validation is not supported by Blazor's <see cref="EditContext" />
///     and <see cref="EditContext.Validate" /> most probably returns wrong result and causes confusion.<br />
///     If provided validators contain <see cref="IAsyncUniqueValidator{TModel}" />, then this component will not set
///     <see cref="EditContext" />'s <see cref="EditContext.OnValidationRequested" /> handler, which is used when from is
///     submitted and validation is performed automatically.
/// </remarks>
public class FluentValidator : ComponentBase
{
    private static string _fluentValidatorName = nameof(FluentValidator);

    private ValidationMessageStore _messageStore = null!;
    private Type _modelType = null!;

    private List<IValidator> _normalAsyncUniqueValidators = new();
    private List<IValidator> _uniqueAsyncValidators = new();

    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = null!;

    [Inject]
    private ILogger<FluentValidator> Logger { get; set; } = null!;

    [CascadingParameter]
    public EditContext CurrentEditContext { get; set; } = null!;

    [CascadingParameter]
    public CancellationToken CT { get; set; }

    [Parameter]
    public IEnumerable<IValidator>? Validators { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (CurrentEditContext is null)
        {
            throw new InvalidOperationException(GetMsgMissingEditContext());
        }

        _modelType = CurrentEditContext.Model.GetType();

        _messageStore = new(CurrentEditContext);

        SetUpValidators();

        SetupEventHandlers();
    }

    public async ValueTask<bool> ValidateModelAsync()
    {
        await ModelValidationHandler();

        return !CurrentEditContext.GetValidationMessages().Any();
    }

    private void SetUpValidators()
    {
        List<IValidator> sourceValidators;
        bool isFromDI;

        if (Validators is not null && Validators.Any())
        {
            sourceValidators = Validators.ToList();
            isFromDI = false;
        }
        else
        {
            sourceValidators = GetValidatorsFromDIContainer();
            isFromDI = true;
        }

        LogValidators(sourceValidators, isFromDI);

        _uniqueAsyncValidators.AddRange(
            sourceValidators.Where(
                validator => validator.GetType().IsAssignableToGenericInterface(typeof(IAsyncUniqueValidator<>))
            )
        );

        _normalAsyncUniqueValidators.AddRange(
            sourceValidators.Except(_uniqueAsyncValidators)
        );
    }

    /// <exception cref="InvalidOperationException">
    ///     In case no validators exist in DI container for <see cref="EditForm" />
    ///     model's type.
    /// </exception>
    private List<IValidator> GetValidatorsFromDIContainer()
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(_modelType);

        return ServiceProvider.GetServices(validatorType).Cast<IValidator>().ToList() is { Count: > 0, } validators
            ? validators
            : throw new InvalidOperationException(GetMsgNoExpectedValidatorsInDIContainer());
    }

    private void SetupEventHandlers()
    {
        CurrentEditContext.OnFieldChanged += async (_, args) => await FieldValidationHandler(args.FieldIdentifier);

        if (_uniqueAsyncValidators.Count == 0)
        {
            CurrentEditContext.OnValidationRequested += async (_, _) => await ModelValidationHandler();
        }
    }

    private ValueTask ModelValidationHandler() =>
        ValidateByStrategyAsync<object>(
            strategy => strategy.IncludeAllRuleSets(),
            messageStore => messageStore.Clear()
        );

    private ValueTask FieldValidationHandler(FieldIdentifier filedIdentifier) =>
        //TODO: Needs throttling in case any validator makes async calls outside of browser!
        ValidateByStrategyAsync<object>(
            strategy => strategy.IncludeProperties(filedIdentifier.FieldName),
            messageStore => messageStore.Clear(filedIdentifier)
        );

    /// <summary>
    ///     Validates this <see cref="EditContext" />.
    /// </summary>
    private async ValueTask ValidateByStrategyAsync<TModel>(
        Action<ValidationStrategy<TModel>> strategy,
        Action<ValidationMessageStore>     clearMessages
    )
    {
        var validationFailures = await GetValidationFailures(strategy);

        clearMessages.Invoke(_messageStore);

        validationFailures.ForEach(
            failure => _messageStore.Add(
                CurrentEditContext.Field(failure.PropertyName),
                failure.ErrorMessage
            )
        );

        CurrentEditContext.NotifyValidationStateChanged();
    }

    /// <summary>
    ///     Will return early, if normal validators fail, because potentially async validator might fail to query against API,
    ///     if value is empty or similar violation.
    /// </summary>
    private async ValueTask<List<ValidationFailure>> GetValidationFailures<TModel>(
        Action<ValidationStrategy<TModel>> strategy
    )
    {
        // First, validate "normal"  validators, that do not implement IAsyncUniqueValidator<object>, only IValidator<object>.
        var failures = ValidateWithNormalValidators(strategy);
        if (failures.Count > 0)
        {
            return failures;
        }

        // Second, validate rest of validators.
        failures.AddRange(await ValidateWithUniqueValidatorsAsync(strategy));

        return failures;
    }

    private List<ValidationFailure> ValidateWithNormalValidators<TModel>(Action<ValidationStrategy<TModel>> strategy)
    {
        var validationResults = _normalAsyncUniqueValidators.Select(
            validator => validator.Validate(ValidationContextFactory(strategy))
        );

        return GetFailuresList(validationResults);
    }

    private async ValueTask<List<ValidationFailure>> ValidateWithUniqueValidatorsAsync<TModel>(
        Action<ValidationStrategy<TModel>> strategy
    )
    {
        var tasks = _uniqueAsyncValidators.Select(
            validator => validator.ValidateAsync(ValidationContextFactory(strategy), CT)
        );

        var validationResults = await Task.WhenAll(tasks);

        return GetFailuresList(validationResults);
    }

    private static List<ValidationFailure> GetFailuresList(IEnumerable<ValidationResult> nonUniqueValidationResults) =>
        nonUniqueValidationResults
            .SelectMany(vr => vr.Errors)
            .Where(failure => failure is not null)
            .ToList();

    private ValidationContext<TModel> ValidationContextFactory<TModel>(Action<ValidationStrategy<TModel>> strategy) =>
        ValidationContext<TModel>.CreateWithOptions((TModel)CurrentEditContext.Model, strategy);

    private string GetMsgMissingEditContext() =>
        $"{_fluentValidatorName} requires a cascading parameter of type {nameof(EditContext)}."
        + $" For example, you can use {_fluentValidatorName} inside an {nameof(EditForm)}.";

    private string GetMsgNoExpectedValidatorsInDIContainer() =>
        $"{_fluentValidatorName} expects that validators are registered in DI container before usage. "
        + $"No validators for type of model '{_modelType.FullName}' where found.";

    private void LogValidators(List<IValidator> sourceValidators, bool isFromDI) =>
        Logger.LogDebug(
            "Got {Count} validators from `{Source}` for model `{ModelType}`: {Validators}",
            sourceValidators.Count,
            isFromDI ? "DI container" : "parameter",
            _modelType.FullName,
            sourceValidators.Select(v => $"\n- {v.GetType().FullName}")
        );
}
