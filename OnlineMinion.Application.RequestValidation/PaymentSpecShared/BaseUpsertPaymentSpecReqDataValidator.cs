using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;

namespace OnlineMinion.Application.RequestValidation.PaymentSpecShared;

[UsedImplicitly]
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

                RuleFor(x => x.Tags)
                    .MaximumLength(150);
            }
        );
    }
}
