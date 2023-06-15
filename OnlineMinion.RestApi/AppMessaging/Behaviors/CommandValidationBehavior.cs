using ErrorOr;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using MediatR;
using OnlineMinion.Contracts.AppMessaging;
using OnlineMinion.RestApi.Validation;

namespace OnlineMinion.RestApi.AppMessaging.Behaviors;

/// <summary>
///     Supports sync and async validators. Takes in all validators for given request type (from DI).
///     Guarantees validation of all rulesets using <see cref="RulesetValidatorSelector.WildcardRuleSetName" />.
/// </summary>
/// <remarks>
///     Type constraints are supposed to defined pipeline "segment" only for command type requests.<br />
///     Validators marked with <see cref="IAsyncUniqueValidator{TModel}" /> are executed first. In case failure occurs,
///     the rest of validators are not executed and pipeline will be short-circuited.<br />
/// </remarks>
/// <typeparam name="TRequest">Request or model, constrained to <see cref="ICommand" />.</typeparam>
/// <typeparam name="TResponse">Response model, constrained to <see cref="IErrorOr" />.</typeparam>
public class CommandValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>[] _validators;

    public CommandValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
        _validators = validators.ToArray();

    public async Task<TResponse> Handle(TRequest req, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (_validators.Length == 0)
        {
            return await next().ConfigureAwait(false);
        }

        ErrorOr<Success> validationResult;

        // First, if there are any uniqueness validators, they must be executed first.
        var uniqueValidators = GetUniqueValidators();

        validationResult = await ValidateAsync(req, uniqueValidators, CreateConflictError, ct)
            .ConfigureAwait(false);

        if (validationResult.IsError)
        {
            return (TResponse)(dynamic)validationResult.Errors;
        }

        // Second, execute all other validators.
        var restOfValidators = _validators.Except(uniqueValidators).ToArray();

        validationResult = await ValidateAsync(req, restOfValidators, CreateValidationError, ct)
            .ConfigureAwait(false);

        return validationResult.IsError
            ? (TResponse)(dynamic)validationResult.Errors
            : await next().ConfigureAwait(false);
    }

    private IAsyncUniqueValidator<TRequest>[] GetUniqueValidators() =>
        _validators.OfType<IAsyncUniqueValidator<TRequest>>().ToArray();

    private static async ValueTask<ErrorOr<Success>> ValidateAsync(
        TRequest                                  req,
        IReadOnlyCollection<IValidator<TRequest>> validators,
        Func<ValidationFailure[], Error>          errFactory,
        CancellationToken                         ct
    )
    {
        if (validators.Count == 0)
        {
            return Result.Success;
        }

        return await GetValidationFailures(req, validators, ct).ConfigureAwait(false) is { Length: > 0, } failures
            ? errFactory(failures)
            : Result.Success;
    }

    private static async ValueTask<ValidationFailure[]> GetValidationFailures(
        TRequest                          req,
        IEnumerable<IValidator<TRequest>> validators,
        CancellationToken                 ct
    )
    {
        var validationResults = await RunAsyncValidators(req, validators, ct).ConfigureAwait(false);

        return validationResults
            .SelectMany(vr => vr.Errors)
            .Where(failure => failure is not null)
            .ToArray();
    }

    private static Task<ValidationResult[]> RunAsyncValidators(
        TRequest                          req,
        IEnumerable<IValidator<TRequest>> validators,
        CancellationToken                 ct
    ) => Task.WhenAll(
        validators.Select(
            validator => validator.ValidateAsync(req, strategy => strategy.IncludeAllRuleSets(), ct)
        )
    );

    private static Error CreateValidationError(ValidationFailure[] failures) =>
        Error.Validation(dictionary: TransformFailuresToDictionary(failures));

    private static Error CreateConflictError(ValidationFailure[] failures) =>
        Error.Conflict(dictionary: TransformFailuresToDictionary(failures));

    private static Dictionary<string, object> TransformFailuresToDictionary(
        IEnumerable<ValidationFailure> validationFailures
    ) => validationFailures
        .GroupBy(
            x => x.PropertyName,
            x => x.ErrorMessage,
            (propertyName, errorMessages) => new
            {
                Key = propertyName ?? string.Empty,
                Values = errorMessages.Distinct(StringComparer.OrdinalIgnoreCase),
            },
            StringComparer.OrdinalIgnoreCase
        )
        .ToDictionary(
            x => x.Key,
            x => x.Values as object, // TODO Stinks!
            StringComparer.OrdinalIgnoreCase
        );
}
