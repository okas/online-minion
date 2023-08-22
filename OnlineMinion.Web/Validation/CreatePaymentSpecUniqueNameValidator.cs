using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Common;
using OnlineMinion.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Web.Validation;

[UsedImplicitly]
public sealed class CreatePaymentSpecUniqueNameValidator : AbstractValidator<CreatePaymentSpecReq>,
    IAsyncUniqueValidator<CreatePaymentSpecReq>
{
    private readonly ISender _sender;

    public CreatePaymentSpecUniqueNameValidator(ISender sender)
    {
        _sender = sender;

        RuleFor(x => x.Name)
            .MustAsync(BeUnique)
            .WithMessage("'{PropertyName}' must be unique");
    }

    private async Task<bool> BeUnique(string name, CancellationToken ct) =>
        await _sender.Send(new CheckPaymentSpecUniqueNewReq(name), ct).ConfigureAwait(false);
}
