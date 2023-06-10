using FluentValidation;

namespace OnlineMinion.Contracts.Validators;

public class HasIntIdValidator : AbstractValidator<IHasIntId>
{
    public HasIntIdValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThanOrEqualTo(1);
    }
}
