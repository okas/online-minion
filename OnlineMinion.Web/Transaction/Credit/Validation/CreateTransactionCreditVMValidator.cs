using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Transactions;
using OnlineMinion.Web.Transaction.Credit.ViewModels;

namespace OnlineMinion.Web.Transaction.Credit.Validation;

[UsedImplicitly]
public sealed class CreateTransactionCreditVMValidator : AbstractValidator<CreateTransactionCreditVM>
{
    public CreateTransactionCreditVMValidator(BaseUpsertTransactionReqDataValidator baseValidator) =>
        Include(baseValidator);
}
