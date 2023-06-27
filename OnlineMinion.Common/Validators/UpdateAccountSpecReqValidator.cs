using FluentValidation;
using OnlineMinion.Contracts.AppMessaging.Requests;

namespace OnlineMinion.Common.Validators;

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
