using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Common.Validation.AccountSpec;

[UsedImplicitly]
public class CreateAccountSpecReqValidator : AbstractValidator<CreateAccountSpecReq>
{
    public CreateAccountSpecReqValidator(BaseUpsertAccountSpecReqDataValidator baseValidator) =>
        Include(baseValidator);
}
