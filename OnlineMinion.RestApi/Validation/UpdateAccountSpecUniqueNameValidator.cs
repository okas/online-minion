using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.Validation;

public sealed class UpdateAccountSpecUniqueNameValidator : AbstractValidator<UpdateAccountSpecReq>,
    IAsyncUniqueValidator<UpdateAccountSpecReq>
{
    private readonly OnlineMinionDbContext _dbContext;

    public UpdateAccountSpecUniqueNameValidator(OnlineMinionDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name)
            .MustAsync(BeUnique)
            .WithMessage("'{PropertyName}' must be unique");
    }

    private async Task<bool> BeUnique(UpdateAccountSpecReq req, string name, CancellationToken ct) =>
        !await _dbContext.AccountSpecs
            .AnyAsync(entity => entity.Id != req.Id && entity.Name == name, ct)
            .ConfigureAwait(false);
}
