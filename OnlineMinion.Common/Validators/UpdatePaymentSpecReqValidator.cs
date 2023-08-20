using FluentValidation;
using OnlineMinion.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Common.Validators;

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
