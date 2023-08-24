using FluentValidation;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Common.AccountSpec.Validators;

public class CreateAccountSpecReqValidator : AbstractValidator<CreateAccountSpecReq>
{
    public CreateAccountSpecReqValidator(BaseUpsertAccountSpecReqDataValidator baseValidator) =>
        Include(baseValidator);
}
