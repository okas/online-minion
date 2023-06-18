using FluentValidation;
using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;

namespace OnlineMinion.RestApi.Validation;

public sealed class CreateAccountSpecUniqueNameValidator : AbstractValidator<CreateAccountSpecReq>,
    IAsyncUniqueValidator<CreateAccountSpecReq>
{
    private readonly ISender _sender;

    public CreateAccountSpecUniqueNameValidator(ISender sender)
    {
        _sender = sender;

        RuleFor(x => x.Name)
            .MustAsync(BeUnique)
            .WithMessage("'{PropertyName}' must be unique");
    }

    // TODO: To interface IAsyncUniqueValidator<TModel>  member?
    private async Task<bool> BeUnique(string name, CancellationToken ct) =>
        await _sender.Send(new CheckAccountSpecUniqueNewReq(name), ct).ConfigureAwait(false);
}
