using ErrorOr;
using FluentValidation;
using MediatR;
using OnlineMinion.Common;

namespace OnlineMinion.RestApi.Validation;

public abstract class BaseCheckUniqueModelValidator<TModel> : AbstractValidator<TModel>, IAsyncUniqueValidator<TModel>
{
    protected const string FailureMessageFormat = "'{PropertyName}' must be unique";

    private readonly ISender _sender;
    protected BaseCheckUniqueModelValidator(ISender sender) => _sender = sender;

    protected async Task<bool> BeUniqueAsync(TModel model, string value, CancellationToken ct)
    {
        var rq = ValidationRequestFactory(model, value);
        var result = await _sender.Send(rq, ct).ConfigureAwait(false);

        return result.MatchFirst(
            _ => true,
            firstError => firstError.Type is ErrorType.Conflict
                ? false
                : throw new ValidationException(
                    "Unexpected error while checking uniqueness of Payment specification name."
                )
        );
    }

    protected abstract IRequest<ErrorOr<Success>> ValidationRequestFactory(TModel model, string value);
}
