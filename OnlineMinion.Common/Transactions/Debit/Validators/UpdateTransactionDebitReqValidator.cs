using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Shared.Validation;
using OnlineMinion.Contracts.Transactions.Debit.Requests;

namespace OnlineMinion.Common.Transactions.Debit.Validators;

[UsedImplicitly]
public sealed class UpdateTransactionDebitReqValidator : AbstractValidator<UpdateTransactionDebitReq>
{
    public UpdateTransactionDebitReqValidator(
        HasIdValidator                             idValidator,
        BaseUpsertTransactionDebitReqDataValidator baseValidator
    )
    {
        Include(idValidator);
        Include(baseValidator);
    }
}
