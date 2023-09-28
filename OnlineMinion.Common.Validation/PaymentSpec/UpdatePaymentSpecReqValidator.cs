using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Validation.Shared;
using OnlineMinion.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Common.Validation.PaymentSpec;

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
