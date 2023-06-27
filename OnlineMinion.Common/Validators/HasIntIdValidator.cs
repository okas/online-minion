using FluentValidation;
using OnlineMinion.Contracts;

namespace OnlineMinion.Common.Validators;

public class HasIntIdValidator : AbstractValidator<IHasIntId>
{
    public HasIntIdValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1);
    }
}
