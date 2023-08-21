using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

internal sealed class CheckAccountSpecUniqueExistingReqHlr : IRequestHandler<CheckAccountSpecUniqueExistingReq, bool>
{
    private readonly OnlineMinionDbContext _dbContext;
    public CheckAccountSpecUniqueExistingReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<bool> Handle(CheckAccountSpecUniqueExistingReq req, CancellationToken ct) =>
        !await _dbContext.AccountSpecs
            .AnyAsync(entity => entity.Name == req.Name && entity.Id != req.ExceptId, ct)
            .ConfigureAwait(false);
}
