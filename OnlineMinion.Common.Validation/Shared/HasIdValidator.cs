using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Contracts;

namespace OnlineMinion.Common.Validation.Shared;

[UsedImplicitly]
public class HasIdValidator : AbstractValidator<IHasId>
{
    public HasIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
