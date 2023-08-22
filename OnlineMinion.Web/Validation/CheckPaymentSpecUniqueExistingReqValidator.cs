using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Common;
using OnlineMinion.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Web.Validation;

[UsedImplicitly]
public class CheckPaymentSpecUniqueExistingReqValidator : AbstractValidator<UpdatePaymentSpecReq>,
    IAsyncUniqueValidator<UpdatePaymentSpecReq>
{
    private readonly ISender _sender;

    public CheckPaymentSpecUniqueExistingReqValidator(ISender sender)
    {
        _sender = sender;

        RuleFor(x => x.Name)
            .MustAsync(BeUnique)
            .WithMessage("'{PropertyName}' must be unique");
    }

    private async Task<bool> BeUnique(UpdatePaymentSpecReq req, string name, CancellationToken ct) =>
        await _sender.Send(new CheckPaymentSpecUniqueExistingReq(name, req.Id), ct).ConfigureAwait(false);
}
