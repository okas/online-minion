using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Application.RequestValidation.AccountSpec;

[UsedImplicitly]
public class CreateAccountSpecReqValidator : AbstractValidator<CreateAccountSpecReq>
{
    public CreateAccountSpecReqValidator(BaseUpsertAccountSpecReqDataValidator baseValidator) =>
        Include(baseValidator);
}
