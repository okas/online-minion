using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Contracts;

namespace OnlineMinion.Common.Shared.Validation;

[UsedImplicitly]
public class HasIntIdValidator : AbstractValidator<IHasId>
{
    public HasIntIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
