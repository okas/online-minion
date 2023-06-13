using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AppMessaging;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.Validators;

public sealed class BaseUpsertAccountSpecUniqueNameValidator : AbstractValidator<BaseUpsertAccountSpecReqData>
{
    private readonly OnlineMinionDbContext _dbContext;

    public BaseUpsertAccountSpecUniqueNameValidator(OnlineMinionDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleSet(
            nameof(BaseUpsertAccountSpecUniqueNameValidator),
            () =>
            {
                RuleFor(x => x.Name)
                    .MustAsync(BeUnique)
                    .WithMessage("'{PropertyName}' must be unique");
            }
        );
    }

    private async Task<bool> BeUnique(string namePropVal, CancellationToken ct) =>
        !await _dbContext.AccountSpecs
            .AnyAsync(entity => entity.Name == namePropVal, ct)
            .ConfigureAwait(false);
}
