using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCrypto.Requests;
using OnlineMinion.Application.RequestValidation.PaymentSpecShared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecCrypto;

[UsedImplicitly]
public sealed class CheckUniqueExistingPaymentSpecCryptoNameReqValidator(IAsyncValidatorSender sender)
    : BaseCheckUniqueExistingPaymentSpecNameReqValidator<UpdatePaymentSpecCryptoReq>(sender)
{
    protected override string ModelName => "Crypto payment specification";
}
