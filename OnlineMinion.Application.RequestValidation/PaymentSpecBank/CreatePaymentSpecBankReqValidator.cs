using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.Application.RequestValidation.PaymentSpecShared;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecBank;

[UsedImplicitly]
public class CreatePaymentSpecBankReqValidator : AbstractValidator<CreatePaymentSpecBankReq>
{
    public CreatePaymentSpecBankReqValidator(
        BaseUpsertPaymentSpecReqDataValidator     baseValidator,
        BaseUpsertPaymentSpecBankReqDataValidator baseBankValidator
    )
    {
        Include(baseValidator);
        Include(baseBankValidator);

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .Length(3);
    }
}
