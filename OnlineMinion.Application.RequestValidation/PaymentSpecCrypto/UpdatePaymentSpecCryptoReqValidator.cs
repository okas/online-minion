using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCrypto.Requests;
using OnlineMinion.Application.RequestValidation.PaymentSpecShared;
using OnlineMinion.Application.RequestValidation.Shared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecCrypto;

[UsedImplicitly]
public class UpdatePaymentSpecCryptoReqValidator : AbstractValidator<UpdatePaymentSpecCryptoReq>
{
    public UpdatePaymentSpecCryptoReqValidator(
        HasIdValidator                              idValidator,
        BaseUpsertPaymentSpecReqDataValidator       baseValidator,
        BaseUpsertPaymentSpecCryptoReqDataValidator baseCryptoValidator
    )
    {
        Include(idValidator);
        Include(baseValidator);
        Include(baseCryptoValidator);
    }
}
