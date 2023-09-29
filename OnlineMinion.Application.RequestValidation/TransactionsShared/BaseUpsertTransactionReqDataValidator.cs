using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions;

namespace OnlineMinion.Application.RequestValidation.TransactionsShared;

[UsedImplicitly]
public sealed class BaseUpsertTransactionReqDataValidator : AbstractValidator<BaseUpsertTransactionReqData>
{
    public BaseUpsertTransactionReqDataValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.PaymentInstrumentId)
            .NotEmpty();

        RuleFor(x => x.Date)
            .NotEmpty();

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

        RuleFor(x => x.Tags)
            .MaximumLength(150);
    }
}
