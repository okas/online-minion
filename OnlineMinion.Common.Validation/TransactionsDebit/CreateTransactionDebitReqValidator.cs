using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Debit.Requests;

namespace OnlineMinion.Common.Validation.TransactionsDebit;

[UsedImplicitly]
public sealed class CreateTransactionDebitReqValidator : AbstractValidator<CreateTransactionDebitReq>
{
    public CreateTransactionDebitReqValidator(BaseUpsertTransactionDebitReqDataValidator baseValidator) =>
        Include(baseValidator);
}
