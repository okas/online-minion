using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class CheckAccountSpecUniqueExistingReqHlr : IRequestHandler<CheckAccountSpecUniqueExistingReq, bool>
{
    private readonly OnlineMinionDbContext _dbContext;
    public CheckAccountSpecUniqueExistingReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<bool> Handle(CheckAccountSpecUniqueExistingReq req, CancellationToken ct) =>
        !await _dbContext.AccountSpecs
            .AnyAsync(entity => entity.Name == req.Name && entity.Id != req.ExceptId, ct)
            .ConfigureAwait(false);
}
