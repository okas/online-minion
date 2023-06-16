using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.Validation;

public sealed class CreateAccountSpecUniqueNameValidator : AbstractValidator<CreateAccountSpecReq>,
    IAsyncUniqueValidator<CreateAccountSpecReq>
{
    private readonly OnlineMinionDbContext _dbContext;

    public CreateAccountSpecUniqueNameValidator(OnlineMinionDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name)
            .MustAsync(BeUnique)
            .WithMessage("'{PropertyName}' must be unique");
    }

    private async Task<bool> BeUnique(CreateAccountSpecReq req, string name, CancellationToken ct) =>
        await _dbContext.AccountSpecs
            .AllAsync(entity => entity.Name != name, ct)
            .ConfigureAwait(false);
}
