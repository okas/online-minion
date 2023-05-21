using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class UpdateAccountSpecReqHlr : IRequestHandler<UpdateAccountSpecReq, bool>
{
    private readonly OnlineMinionDbContext _dbContext;

    public UpdateAccountSpecReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<bool> Handle(UpdateAccountSpecReq rq, CancellationToken ct)
    {
        var updatedCount = await _dbContext.AccountSpecs
            .Where(a => a.Id == rq.Id)
            .ExecuteUpdateAsync(
                s => s
                    .SetProperty(a => a.Name, rq.Name)
                    .SetProperty(a => a.Group, rq.Group)
                    .SetProperty(a => a.Description, rq.Description),
                ct
            )
            .ConfigureAwait(false);

        return updatedCount > 0;
    }
}
