using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Contracts.Transactions.Debit.Requests;

namespace OnlineMinion.Common.Transactions.Debit.Validators;

[UsedImplicitly]
public sealed class CreateTransactionDebitReqValidator : AbstractValidator<CreateTransactionDebitReq>
{
    public CreateTransactionDebitReqValidator(BaseUpsertTransactionDebitReqDataValidator baseValidator) =>
        Include(baseValidator);
}
