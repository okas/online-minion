using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Validation.TransactionsShared;
using OnlineMinion.Contracts.Transactions.Debit;

namespace OnlineMinion.Common.Validation.TransactionsDebit;

[UsedImplicitly]
public sealed class BaseUpsertTransactionDebitReqDataValidator : AbstractValidator<BaseUpsertTransactionDebitReqData>
{
    public BaseUpsertTransactionDebitReqDataValidator(BaseUpsertTransactionReqDataValidator baseValidator)
    {
        Include(baseValidator);

        RuleFor(x => x.AccountSpecId)
            .NotEmpty();

        RuleFor(x => x.Fee)
            .GreaterThanOrEqualTo(0);
    }
}
