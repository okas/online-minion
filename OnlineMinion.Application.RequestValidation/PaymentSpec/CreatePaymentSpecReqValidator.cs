using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpec.Requests;

namespace OnlineMinion.Application.RequestValidation.PaymentSpec;

[UsedImplicitly]
public class CreatePaymentSpecReqValidator : AbstractValidator<CreatePaymentSpecReq>
{
    public CreatePaymentSpecReqValidator(BaseUpsertPaymentSpecReqDataValidator baseValidator) =>
        Include(baseValidator);
}
