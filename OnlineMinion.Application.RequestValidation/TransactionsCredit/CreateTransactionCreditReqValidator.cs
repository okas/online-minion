using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Application.RequestValidation.TransactionsShared;

namespace OnlineMinion.Application.RequestValidation.TransactionsCredit;

[UsedImplicitly]
public sealed class CreateTransactionCreditReqValidator : AbstractValidator<CreateTransactionCreditReq>
{
    public CreateTransactionCreditReqValidator(BaseUpsertTransactionReqDataValidator baseValidator) =>
        Include(baseValidator);
}
