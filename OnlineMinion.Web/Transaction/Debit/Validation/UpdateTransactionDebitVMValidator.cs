using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Shared.Validation;
using OnlineMinion.Common.Transactions;
using OnlineMinion.Web.Transaction.Debit.ViewModels;

namespace OnlineMinion.Web.Transaction.Debit.Validation;

[UsedImplicitly]
public sealed class UpdateTransactionDebitVMValidator : AbstractValidator<UpdateTransactionDebitVM>
{
    public UpdateTransactionDebitVMValidator(
        HasIntIdValidator                     intIdValidator,
        BaseUpsertTransactionReqDataValidator baseValidator
    )
    {
        Include(intIdValidator);
        Include(baseValidator);

        // It is important, because it is hidden member, thus base validator wont reach it.
        RuleFor(x => x.Date)
            .NotEmpty();
    }
}
