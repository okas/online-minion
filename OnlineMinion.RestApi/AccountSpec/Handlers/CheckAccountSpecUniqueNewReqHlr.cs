using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

internal sealed class CheckAccountSpecUniqueNewReqHlr : IRequestHandler<CheckAccountSpecUniqueNewReq, bool>
{
    private readonly OnlineMinionDbContext _dbContext;
    public CheckAccountSpecUniqueNewReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<bool> Handle(CheckAccountSpecUniqueNewReq req, CancellationToken ct) =>
        await _dbContext.AccountSpecs
            .AllAsync(entity => entity.Name != req.Name, ct)
            .ConfigureAwait(false);
}
