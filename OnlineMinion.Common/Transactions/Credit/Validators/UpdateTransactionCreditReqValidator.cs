using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Shared.Validation;
using OnlineMinion.Contracts.Transactions.Credit.Requests;

namespace OnlineMinion.Common.Transactions.Credit.Validators;

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
