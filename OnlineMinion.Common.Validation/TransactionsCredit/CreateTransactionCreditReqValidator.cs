using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Validation.TransactionsShared;
using OnlineMinion.Contracts.Transactions.Credit.Requests;

namespace OnlineMinion.Common.Validation.TransactionsCredit;

[UsedImplicitly]
public sealed class CreateTransactionCreditReqValidator : AbstractValidator<CreateTransactionCreditReq>
{
    public CreateTransactionCreditReqValidator(BaseUpsertTransactionReqDataValidator baseValidator) =>
        Include(baseValidator);
}
