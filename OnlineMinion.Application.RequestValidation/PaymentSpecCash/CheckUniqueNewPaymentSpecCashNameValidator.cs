using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.Application.RequestValidation.PaymentSpecShared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecCash;

[UsedImplicitly]
public sealed class CheckUniqueNewPaymentSpecCashNameValidator(IAsyncValidatorSender sender)
    : BaseCheckUniqueNewPaymentSpecNameValidator<CreatePaymentSpecCashReq>(sender)
{
    protected override string ModelName => "Cash payment specification";
}
