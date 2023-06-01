using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class UpdateAccountSpecReqHlr : IRequestHandler<UpdateAccountSpecReq, Result<bool>>
{
    private readonly OnlineMinionDbContext _dbContext;

    public UpdateAccountSpecReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<Result<bool>> Handle(UpdateAccountSpecReq rq, CancellationToken ct)
    {
        // TODO Handle validation and/or exception logic here

        var accountSpec = await _dbContext.AccountSpecs.FindAsync(new object?[] { rq.Id, }, ct).ConfigureAwait(false);

        accountSpec!.Name = rq.Name;
        accountSpec.Group = rq.Group;
        accountSpec.Description = rq.Description;

        var updatedCount = await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);

        return updatedCount > 0;
    }
}
