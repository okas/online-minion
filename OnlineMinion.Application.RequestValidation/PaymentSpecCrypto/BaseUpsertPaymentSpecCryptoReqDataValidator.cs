using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCrypto;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecCrypto;

[UsedImplicitly]
public class BaseUpsertPaymentSpecCryptoReqDataValidator : AbstractValidator<IUpsertPaymentSpecCryptoReq>
{
    public BaseUpsertPaymentSpecCryptoReqDataValidator()
    {
        RuleFor(x => x.ExchangeName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(75);

        RuleFor(x => x.IsFiat)
            .NotNull();
    }
}
