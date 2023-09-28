using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Validation.TransactionsShared;
using OnlineMinion.SPA.Blazor.Transaction.Debit.ViewModels;

namespace OnlineMinion.SPA.Blazor.Transaction.Debit.Validation;

[UsedImplicitly]
public sealed class CreateTransactionDebitVMValidator : AbstractValidator<CreateTransactionDebitVM>
{
    public CreateTransactionDebitVMValidator(BaseUpsertTransactionReqDataValidator baseValidator) =>
        Include(baseValidator);
}
