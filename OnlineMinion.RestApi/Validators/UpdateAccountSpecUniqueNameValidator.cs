using FluentValidation;
using OnlineMinion.Contracts.AppMessaging.Requests;

namespace OnlineMinion.RestApi.Validators;

public sealed class UpdateAccountSpecUniqueNameValidator : AbstractValidator<UpdateAccountSpecReq>
{
    public UpdateAccountSpecUniqueNameValidator(BaseUpsertAccountSpecUniqueNameValidator baseValidator) =>
        Include(baseValidator);
}
