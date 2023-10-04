using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.Application.RequestValidation.PaymentSpecShared;
using OnlineMinion.Application.RequestValidation.Shared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecCash;

[UsedImplicitly]
public class UpdatePaymentSpecCashReqValidator : AbstractValidator<UpdatePaymentSpecCashReq>
{
    public UpdatePaymentSpecCashReqValidator(
        HasIdValidator                        idValidator,
        BaseUpsertPaymentSpecReqDataValidator baseValidator
    )
    {
        Include(idValidator);
        Include(baseValidator);
    }
}
