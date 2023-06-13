using FluentValidation;
using OnlineMinion.Contracts.AppMessaging.Requests;

namespace OnlineMinion.Contracts.Validators;

public class UpdateAccountSpecReqValidator : AbstractValidator<UpdateAccountSpecReq>
{
    public UpdateAccountSpecReqValidator(
        HasIntIdValidator                     intIdValidator,
        BaseUpsertAccountSpecReqDataValidator baseValidator
    )
    {
        Include(intIdValidator);
        Include(baseValidator);
    }
}
