using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.Application.RequestValidation.PaymentSpecShared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecBank;

[UsedImplicitly]
public sealed class CheckUniqueExistingPaymentSpecBankNameReqValidator(IAsyncValidatorSender sender)
    : BaseCheckUniqueExistingPaymentSpecNameReqValidator<UpdatePaymentSpecBankReq>(sender)
{
    protected override string ModelName => "Bank payment specification";
}
