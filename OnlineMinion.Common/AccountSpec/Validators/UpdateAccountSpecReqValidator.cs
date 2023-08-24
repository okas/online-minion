using FluentValidation;
using OnlineMinion.Common.Common.Validation;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Common.AccountSpec.Validators;

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
