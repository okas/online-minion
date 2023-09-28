using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Validation.Shared;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Common.Validation.AccountSpec;

[UsedImplicitly]
public class UpdateAccountSpecReqValidator : AbstractValidator<UpdateAccountSpecReq>
{
    public UpdateAccountSpecReqValidator(
        HasIdValidator                        idValidator,
        BaseUpsertAccountSpecReqDataValidator baseValidator
    )
    {
        Include(idValidator);
        Include(baseValidator);
    }
}
