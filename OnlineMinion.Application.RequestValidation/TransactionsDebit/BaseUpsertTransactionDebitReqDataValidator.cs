using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Debit;
using OnlineMinion.Application.RequestValidation.TransactionsShared;

namespace OnlineMinion.Application.RequestValidation.TransactionsDebit;

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
