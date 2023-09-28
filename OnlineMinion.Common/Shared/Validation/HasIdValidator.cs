using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Contracts;

namespace OnlineMinion.Common.Shared.Validation;

[UsedImplicitly]
public class HasIdValidator : AbstractValidator<IHasId>
{
    public HasIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
