using FluentValidation;
using OnlineMinion.Contracts.AppMessaging.Requests;

namespace OnlineMinion.RestApi.Validators;

public sealed class CreateAccountSpecUniqueNameValidator : AbstractValidator<CreateAccountSpecReq>
{
    public CreateAccountSpecUniqueNameValidator(BaseUpsertAccountSpecUniqueNameValidator baseValidator) =>
        Include(baseValidator);
}
