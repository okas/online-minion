using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Shared.Validation;
using OnlineMinion.Common.Transactions;
using OnlineMinion.Web.Transaction.Credit.ViewModels;

namespace OnlineMinion.Web.Transaction.Credit.Validation;

[UsedImplicitly]
public sealed class UpdateTransactionCreditVMValidator : AbstractValidator<UpdateTransactionCreditVM>
{
    public UpdateTransactionCreditVMValidator(
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
