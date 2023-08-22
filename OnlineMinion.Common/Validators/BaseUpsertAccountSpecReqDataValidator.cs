using FluentValidation;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Common.Validators;

/// <summary>
///     It is only meant to be be used ad "Included" set of rules to <see cref="BaseUpsertAccountSpecReqData" />
///     inheritors. It is not abstract, because for it's usage instantiation is required.
/// </summary>
public sealed class BaseUpsertAccountSpecReqDataValidator : AbstractValidator<BaseUpsertAccountSpecReqData>
{
    public BaseUpsertAccountSpecReqDataValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleSet(
            nameof(BaseUpsertAccountSpecReqData),
            () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(50);

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
