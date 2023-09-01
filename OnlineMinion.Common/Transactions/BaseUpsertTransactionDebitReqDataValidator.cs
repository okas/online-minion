using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Debit;

namespace OnlineMinion.Common.Transactions;

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
