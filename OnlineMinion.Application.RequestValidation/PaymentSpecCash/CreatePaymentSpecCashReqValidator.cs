using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.Application.RequestValidation.PaymentSpecShared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecCash;

[UsedImplicitly]
public class CreatePaymentSpecCashReqValidator : AbstractValidator<CreatePaymentSpecCashReq>
{
    public CreatePaymentSpecCashReqValidator(BaseUpsertPaymentSpecReqDataValidator baseValidator)
    {
        Include(baseValidator);

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .Length(3);
    }
}
