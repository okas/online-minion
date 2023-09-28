using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Common.Shared.Validation;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Common.AccountSpec.Validators;

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
