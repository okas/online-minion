using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCrypto.Requests;
using OnlineMinion.Application.RequestValidation.PaymentSpecShared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecCrypto;

[UsedImplicitly]
public class CreatePaymentSpecCryptoReqValidator : AbstractValidator<CreatePaymentSpecCryptoReq>
{
    public CreatePaymentSpecCryptoReqValidator(
        BaseUpsertPaymentSpecReqDataValidator       baseValidator,
        BaseUpsertPaymentSpecCryptoReqDataValidator baseCryptoValidator
    )
    {
        Include(baseValidator);
        Include(baseCryptoValidator);

        // TODO: it is not sufficient, as longes can be even 23 chars?
        // Problem for now is that they are stored in the same field by Bank and Crypto! Needs to be fixed!
        // Using 3 for now, as it is used in database too.
        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .Length(3);
    }
}
