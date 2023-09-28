using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Validation.Shared;
using OnlineMinion.Contracts.Transactions.Debit.Requests;

namespace OnlineMinion.Common.Validation.TransactionsDebit;

[UsedImplicitly]
public sealed class UpdateTransactionDebitReqValidator : AbstractValidator<UpdateTransactionDebitReq>
{
    public UpdateTransactionDebitReqValidator(
        HasIdValidator                             idValidator,
        BaseUpsertTransactionDebitReqDataValidator baseValidator
    )
    {
        Include(idValidator);
        Include(baseValidator);
    }
}
