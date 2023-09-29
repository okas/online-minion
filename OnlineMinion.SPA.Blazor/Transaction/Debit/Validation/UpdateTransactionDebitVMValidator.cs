using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.RequestValidation.Shared;
using OnlineMinion.Application.RequestValidation.TransactionsShared;
using OnlineMinion.SPA.Blazor.Transaction.Debit.ViewModels;

namespace OnlineMinion.SPA.Blazor.Transaction.Debit.Validation;

[UsedImplicitly]
public sealed class UpdateTransactionDebitVMValidator : AbstractValidator<UpdateTransactionDebitVM>
{
    public UpdateTransactionDebitVMValidator(
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
