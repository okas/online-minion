using FluentValidation;
using OnlineMinion.Contracts.AppMessaging.Requests;

namespace OnlineMinion.Common.Validators;

public class CreateAccountSpecReqValidator : AbstractValidator<CreateAccountSpecReq>
{
    public CreateAccountSpecReqValidator(BaseUpsertAccountSpecReqDataValidator baseValidator) =>
        Include(baseValidator);
}
