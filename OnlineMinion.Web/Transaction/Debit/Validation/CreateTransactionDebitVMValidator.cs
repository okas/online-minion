using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Transactions;
using OnlineMinion.Web.Transaction.Debit.ViewModels;

namespace OnlineMinion.Web.Transaction.Debit.Validation;

[UsedImplicitly]
public sealed class CreateTransactionDebitVMValidator : AbstractValidator<CreateTransactionDebitVM>
{
    public CreateTransactionDebitVMValidator(BaseUpsertTransactionReqDataValidator baseValidator) =>
        Include(baseValidator);
}
