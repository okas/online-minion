using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpec.Requests;
using OnlineMinion.Application.RequestValidation.Shared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpec;

[UsedImplicitly]
public class UpdatePaymentSpecReqValidator : AbstractValidator<UpdatePaymentSpecReq>
{
    public UpdatePaymentSpecReqValidator(
        HasIdValidator                        idValidator,
        BaseUpsertPaymentSpecReqDataValidator baseValidator
    )
    {
        Include(idValidator);
        Include(baseValidator);
    }
}
