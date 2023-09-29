using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.Transactions.Credit.Requests;
using OnlineMinion.Application.RequestValidation.Shared;
using OnlineMinion.Application.RequestValidation.TransactionsShared;

namespace OnlineMinion.Application.RequestValidation.TransactionsCredit;

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
