using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Common.PaymentSpec.Validators;

[UsedImplicitly]
public class CreatePaymentSpecReqValidator : AbstractValidator<CreatePaymentSpecReq>
{
    public CreatePaymentSpecReqValidator(BaseUpsertPaymentSpecReqDataValidator baseValidator) =>
        Include(baseValidator);
}
