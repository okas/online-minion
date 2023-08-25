using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Shared.Validation;
using OnlineMinion.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Common.PaymentSpec.Validators;

[UsedImplicitly]
public class UpdatePaymentSpecReqValidator : AbstractValidator<UpdatePaymentSpecReq>
{
    public UpdatePaymentSpecReqValidator(
        HasIntIdValidator                     intIdValidator,
        BaseUpsertPaymentSpecReqDataValidator baseValidator
    )
    {
        Include(intIdValidator);
        Include(baseValidator);
    }
}
