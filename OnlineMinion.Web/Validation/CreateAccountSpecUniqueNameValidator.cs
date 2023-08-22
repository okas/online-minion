using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Common;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Web.Validation;

[UsedImplicitly]
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

    private async Task<bool> BeUnique(string name, CancellationToken ct) =>
        await _sender.Send(new CheckAccountSpecUniqueNewReq(name), ct).ConfigureAwait(false);
}
