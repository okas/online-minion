using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Shared.Validation;
using OnlineMinion.Common.Transactions;
using OnlineMinion.SPA.Blazor.Transaction.Credit.ViewModels;

namespace OnlineMinion.SPA.Blazor.Transaction.Credit.Validation;

[UsedImplicitly]
public sealed class UpdateTransactionCreditVMValidator : AbstractValidator<UpdateTransactionCreditVM>
{
    public UpdateTransactionCreditVMValidator(
        HasIdValidator                        idValidator,
        BaseUpsertTransactionReqDataValidator baseValidator
    )
    {
        Include(idValidator);
        Include(baseValidator);

        // It is important, because it is hidden member, thus base validator wont reach it.
        RuleFor(x => x.Date)
            .NotEmpty();
    }
}
