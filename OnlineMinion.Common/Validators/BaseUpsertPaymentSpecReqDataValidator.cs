using FluentValidation;
using OnlineMinion.Contracts.AppMessaging;

namespace OnlineMinion.Common.Validators;

public sealed class BaseUpsertPaymentSpecReqDataValidator : AbstractValidator<BaseUpsertPaymentSpecReqData>
{
    public BaseUpsertPaymentSpecReqDataValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleSet(
            nameof(BaseUpsertPaymentSpecReqData),
            () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(50);

                RuleFor(x => x.CurrencyCode)
                    .NotEmpty()
                    .Length(3);

                RuleFor(x => x.Tags)
                    .MaximumLength(150);
            }
        );
    }
}
