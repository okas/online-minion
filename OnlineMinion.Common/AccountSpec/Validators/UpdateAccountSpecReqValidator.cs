using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Common.Validation;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Common.AccountSpec.Validators;

[UsedImplicitly]
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
