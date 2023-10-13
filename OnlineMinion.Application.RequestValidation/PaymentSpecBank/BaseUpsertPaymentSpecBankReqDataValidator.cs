using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecBank;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecBank;

[UsedImplicitly]
public class BaseUpsertPaymentSpecBankReqDataValidator : AbstractValidator<IUpsertPaymentSpecBankReq>
{
    public BaseUpsertPaymentSpecBankReqDataValidator()
    {
        RuleFor(x => x.IBAN)
            .NotEmpty()
            .Length(16, 34);

        RuleFor(x => x.BankName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(75);
    }
}
