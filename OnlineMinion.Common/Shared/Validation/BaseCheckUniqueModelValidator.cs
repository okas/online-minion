using ErrorOr;
using FluentValidation;
using MediatR;

namespace OnlineMinion.Common.Shared.Validation;

public abstract class BaseCheckUniqueModelValidator<TModel> : AbstractValidator<TModel>, IAsyncUniqueValidator<TModel>
{
    protected const string FailureMessageFormat = "'{PropertyName}' must be unique";

    private readonly ISender _sender;
    protected BaseCheckUniqueModelValidator(ISender sender) => _sender = sender;

    protected abstract string ModelName { get; }

    protected async Task<bool> BeUniqueAsync(TModel model, string value, CancellationToken ct)
    {
        var rq = ValidationRequestFactory(model, value);
        var result = await _sender.Send(rq, ct).ConfigureAwait(false);

        return result.MatchFirst(
            _ => true,
            firstError => firstError.Type is ErrorType.Conflict
                ? false
                : throw new ValidationException($"Unexpected error while checking uniqueness of `{ModelName}`.")
        );
    }

    protected abstract IRequest<ErrorOr<Success>> ValidationRequestFactory(TModel model, string value);
}