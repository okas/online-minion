using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Debit.Requests;

namespace OnlineMinion.Application.RequestValidation.TransactionsDebit;

[UsedImplicitly]
public sealed class CreateTransactionDebitReqValidator : AbstractValidator<CreateTransactionDebitReq>
{
    public CreateTransactionDebitReqValidator(BaseUpsertTransactionDebitReqDataValidator baseValidator) =>
        Include(baseValidator);
}
