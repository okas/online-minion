using FluentValidation;
using OnlineMinion.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Common.Validators;

public class CreatePaymentSpecReqValidator : AbstractValidator<CreatePaymentSpecReq>
{
    public CreatePaymentSpecReqValidator(BaseUpsertPaymentSpecReqDataValidator baseValidator) => Include(baseValidator);
}
