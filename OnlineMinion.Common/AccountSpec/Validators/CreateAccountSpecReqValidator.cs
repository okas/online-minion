using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Common.AccountSpec.Validators;

[UsedImplicitly]
public class CreateAccountSpecReqValidator : AbstractValidator<CreateAccountSpecReq>
{
    public CreateAccountSpecReqValidator(BaseUpsertAccountSpecReqDataValidator baseValidator) =>
        Include(baseValidator);
}
