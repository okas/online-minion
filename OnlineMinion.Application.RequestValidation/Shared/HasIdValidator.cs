using FluentValidation;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts;

namespace OnlineMinion.Application.RequestValidation.Shared;

[UsedImplicitly]
public class HasIdValidator : AbstractValidator<IHasId>
{
    public HasIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
