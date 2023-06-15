using FluentValidation;
using OnlineMinion.Contracts.AppMessaging.Requests;

namespace OnlineMinion.RestApi.Validation;

public sealed class UpdateAccountSpecUniqueNameValidator : AbstractValidator<UpdateAccountSpecReq>,
    IAsyncUniqueValidator<UpdateAccountSpecReq>
{
    public UpdateAccountSpecUniqueNameValidator(BaseUpsertAccountSpecUniqueNameValidator baseValidator) =>
        Include(baseValidator);
}
