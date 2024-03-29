using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.RequestValidation.TransactionsShared;
using OnlineMinion.SPA.Blazor.Transaction.Credit.ViewModels;

namespace OnlineMinion.SPA.Blazor.Transaction.Credit.Validation;

[UsedImplicitly]
public sealed class CreateTransactionCreditVMValidator : AbstractValidator<CreateTransactionCreditVM>
{
    public CreateTransactionCreditVMValidator(BaseUpsertTransactionReqDataValidator baseValidator) =>
        Include(baseValidator);
}
