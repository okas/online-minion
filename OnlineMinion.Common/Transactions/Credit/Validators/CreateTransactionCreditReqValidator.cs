using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Transactions.Common;
using OnlineMinion.Contracts.Transactions.Credit.Requests;

namespace OnlineMinion.Common.Transactions.Credit.Validators;

[UsedImplicitly]
public sealed class CreateTransactionCreditReqValidator : AbstractValidator<CreateTransactionCreditReq>
{
    public CreateTransactionCreditReqValidator(BaseUpsertTransactionReqDataValidator baseValidator) =>
        Include(baseValidator);
}
