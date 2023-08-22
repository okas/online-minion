using FluentValidation;
using MediatR;
using OnlineMinion.Common;

namespace OnlineMinion.RestApi.Validation;

public abstract class BaseCreateModelUniqueNameValidator<TModel> : AbstractValidator<TModel>,
    IAsyncUniqueValidator<TModel>
{
    protected const string FailureMessageFormat = "'{PropertyName}' must be unique";

    private readonly ISender _sender;
    protected BaseCreateModelUniqueNameValidator(ISender sender) => _sender = sender;

    protected async Task<bool> BeUnique(TModel model, string value, CancellationToken ct)
    {
        var request = ValidationRequestFactory(model, value);
        return await _sender.Send(request, ct).ConfigureAwait(false);
    }

    protected abstract IRequest<bool> ValidationRequestFactory(TModel model, string value);
}
