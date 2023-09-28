using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Validation.Shared;
using OnlineMinion.Common.Validation.TransactionsShared;
using OnlineMinion.Contracts.Transactions.Credit.Requests;

namespace OnlineMinion.Common.Validation.TransactionsCredit;

[UsedImplicitly]
public sealed class UpdateTransactionCreditReqValidator : AbstractValidator<UpdateTransactionCreditReq>
{
    public UpdateTransactionCreditReqValidator(
        HasIdValidator                        idValidator,
        BaseUpsertTransactionReqDataValidator baseValidator
    )
    {
        Include(idValidator);
        Include(baseValidator);
    }
}
