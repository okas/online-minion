using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions;

namespace OnlineMinion.Common.Transactions.Common;

[UsedImplicitly]
public sealed class BaseUpsertTransactionReqDataValidator : AbstractValidator<BaseUpsertTransactionReqData>
{
    public BaseUpsertTransactionReqDataValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0.00M);

        RuleFor(x => x.Subject)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.Party)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(75);

        RuleFor(x => x.PaymentInstrumentId)
            .NotEmpty();

        RuleFor(x => x.Tags)
            .MaximumLength(150);
    }
}
