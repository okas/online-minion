using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Contracts;

namespace OnlineMinion.Common.Common.Validation;

[UsedImplicitly]
public class HasIntIdValidator : AbstractValidator<IHasIntId>
{
    public HasIntIdValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1);
    }
}
