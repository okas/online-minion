using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.Application.RequestValidation.PaymentSpecShared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecBank;

[UsedImplicitly]
public sealed class CheckUniqueNewPaymentSpecBankNameValidator(IAsyncValidatorSender sender)
    : BaseCheckUniqueNewPaymentSpecNameValidator<CreatePaymentSpecBankReq>(sender)
{
    protected override string ModelName => "Bank payment specification";
}
