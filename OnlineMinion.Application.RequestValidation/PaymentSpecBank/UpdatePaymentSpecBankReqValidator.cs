using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.Application.RequestValidation.PaymentSpecShared;
using OnlineMinion.Application.RequestValidation.Shared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecBank;

[UsedImplicitly]
public class UpdatePaymentSpecBankReqValidator : AbstractValidator<UpdatePaymentSpecBankReq>
{
    public UpdatePaymentSpecBankReqValidator(
        HasIdValidator                            idValidator,
        BaseUpsertPaymentSpecReqDataValidator     baseValidator,
        BaseUpsertPaymentSpecBankReqDataValidator baseBankValidator
    )
    {
        Include(idValidator);
        Include(baseValidator);
        Include(baseBankValidator);
    }
}
