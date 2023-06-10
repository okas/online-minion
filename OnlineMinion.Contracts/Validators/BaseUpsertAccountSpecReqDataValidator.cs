using FluentValidation;
using OnlineMinion.Contracts.AppMessaging;

namespace OnlineMinion.Contracts.Validators;

public class BaseUpsertAccountSpecReqDataValidator : AbstractValidator<BaseUpsertAccountSpecReqData>
{
    public BaseUpsertAccountSpecReqDataValidator()
    {
        RuleSet(
            nameof(BaseUpsertAccountSpecReqData),
            () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .Length(2, 50);

                RuleFor(x => x.Group)
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(50);

                RuleFor(x => x.Description)
                    .MaximumLength(150);
            }
        );
    }
}
