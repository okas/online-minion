using ErrorOr;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using MediatR;
using OnlineMinion.Contracts.AppMessaging;

namespace OnlineMinion.RestApi.AppMessaging.Behaviors;

/// <summary>
///     Supports sync and async validators. Takes in all validators for given request type (from DI).
///     Guarantees validation of all rulesets using <see cref="RulesetValidatorSelector.WildcardRuleSetName" />.
/// </summary>
/// <remarks>
///     Type constraints are supposed to defined pipeline "segment" only for command type requests.
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
        if (_validators.Length != 0 && await Validate(req, _validators, ct).ConfigureAwait(false) is { } dictionary)
        {
            return (TResponse)(dynamic)Error.Validation(dictionary: dictionary);
        }

        return await next().ConfigureAwait(false);
    }

    private static async ValueTask<Dictionary<string, object>?> Validate(
        TRequest                          req,
        IEnumerable<IValidator<TRequest>> validators,
        CancellationToken                 ct
    )
    {
        // TODO : Analyze AsyncState proprty of ValidationContext!
        // TODO: Using same context causes duplicate validation failures? Investigate!
        var context = ValidationContext<TRequest>.CreateWithOptions(req, strategy => strategy.IncludeAllRuleSets());

        // TODO: IErrorOr creation must happen here, based on validation failure info!
        var validationResults = await Task.WhenAll(
                validators.Select(x => x.ValidateAsync(context, ct))
            )
            .ConfigureAwait(false);

        var validationFailures = validationResults
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToArray();

        return validationFailures.Length == 0 ? null : TransformFailuresToDictionary(validationFailures);
    }

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
