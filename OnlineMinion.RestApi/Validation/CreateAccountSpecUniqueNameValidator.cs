using FluentValidation;
using OnlineMinion.Contracts.AppMessaging.Requests;

namespace OnlineMinion.RestApi.Validation;

public sealed class CreateAccountSpecUniqueNameValidator : AbstractValidator<CreateAccountSpecReq>,
    IAsyncUniqueValidator<CreateAccountSpecReq>
{
    public CreateAccountSpecUniqueNameValidator(BaseUpsertAccountSpecUniqueNameValidator baseValidator) =>
        Include(baseValidator);
}
