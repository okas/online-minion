using FluentValidation;
using OnlineMinion.Contracts.AppMessaging.Requests;

namespace OnlineMinion.Contracts.Validations;

public class UpdateAccountSpecReqValidator : AbstractValidator<UpdateAccountSpecReq>
{
    public UpdateAccountSpecReqValidator()
    {
        Include(new HasIntIdValidator());

        Include(new BaseUpsertAccountSpecReqDataValidation());
    }
}
