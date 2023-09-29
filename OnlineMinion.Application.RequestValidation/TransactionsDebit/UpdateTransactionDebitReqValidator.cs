using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Debit.Requests;
using OnlineMinion.Application.RequestValidation.Shared;

namespace OnlineMinion.Application.RequestValidation.TransactionsDebit;

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
