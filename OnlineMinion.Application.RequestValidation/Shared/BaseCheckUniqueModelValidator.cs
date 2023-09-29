using ErrorOr;
using FluentValidation;
using MediatR;
using OnlineMinion.Common;

namespace OnlineMinion.Application.RequestValidation.Shared;

public abstract class BaseCheckUniqueModelValidator<TModel>(IAsyncValidatorSender sender)
    : AbstractValidator<TModel>, IAsyncUniqueValidator<TModel>
{
    protected const string FailureMessageFormat = "'{PropertyName}' must be unique";

    protected abstract string ModelName { get; }

    protected async Task<bool> BeUniqueAsync(TModel model, string value, CancellationToken ct)
    {
        var rq = ValidationRequestFactory(model, value);
        var result = await sender.Send(rq, ct).ConfigureAwait(false);

        return result.MatchFirst(
            _ => true,
            firstError => firstError.Type is ErrorType.Conflict
                ? false
                : throw new ValidationException($"Unexpected error while checking uniqueness of `{ModelName}`.")
        );
    }

    protected abstract IRequest<ErrorOr<Success>> ValidationRequestFactory(TModel model, string value);
}
