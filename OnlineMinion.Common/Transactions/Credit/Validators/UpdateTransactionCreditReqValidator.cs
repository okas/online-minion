using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Shared.Validation;
using OnlineMinion.Common.Transactions.Common;
using OnlineMinion.Contracts.Transactions.Credit.Requests;

namespace OnlineMinion.Common.Transactions.Credit.Validators;

[UsedImplicitly]
public sealed class UpdateTransactionCreditReqValidator : AbstractValidator<UpdateTransactionCreditReq>
{
    public UpdateTransactionCreditReqValidator(
        HasIntIdValidator                     intIdValidator,
        BaseUpsertTransactionReqDataValidator baseValidator
    )
    {
        Include(intIdValidator);
        Include(baseValidator);
    }
}
