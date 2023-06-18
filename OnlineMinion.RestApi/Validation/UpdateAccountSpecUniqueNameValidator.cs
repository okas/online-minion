using FluentValidation;
using MediatR;
using OnlineMinion.Contracts.AppMessaging.Requests;

namespace OnlineMinion.RestApi.Validation;

public sealed class UpdateAccountSpecUniqueNameValidator : AbstractValidator<UpdateAccountSpecReq>,
    IAsyncUniqueValidator<UpdateAccountSpecReq>
{
    private readonly ISender _sender;

    public UpdateAccountSpecUniqueNameValidator(ISender sender)
    {
        _sender = sender;

        RuleFor(x => x.Name)
            .MustAsync(BeUnique)
            .WithMessage("'{PropertyName}' must be unique");
    }

    // TODO: To interface IAsyncUniqueValidator<TModel>  member?
    private async Task<bool> BeUnique(UpdateAccountSpecReq req, string name, CancellationToken ct) =>
        await _sender.Send(new CheckAccountSpecUniqueExistingReq(name, req.Id), ct).ConfigureAwait(false);
}
