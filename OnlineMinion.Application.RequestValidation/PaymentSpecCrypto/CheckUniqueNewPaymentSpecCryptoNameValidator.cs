using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCrypto.Requests;
using OnlineMinion.Application.RequestValidation.PaymentSpecShared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecCrypto;

[UsedImplicitly]
public sealed class CheckUniqueNewPaymentSpecCryptoNameValidator(IAsyncValidatorSender sender)
    : BaseCheckUniqueNewPaymentSpecNameValidator<CreatePaymentSpecCryptoReq>(sender)
{
    protected override string ModelName => "Crypto payment specification";
}
