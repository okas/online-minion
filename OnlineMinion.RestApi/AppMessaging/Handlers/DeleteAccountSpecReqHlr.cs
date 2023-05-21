using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class DeleteAccountSpecReqHlr : IRequestHandler<DeleteAccountSpecReq, bool>
{
    private readonly OnlineMinionDbContext _dbContext;

    public DeleteAccountSpecReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<bool> Handle(DeleteAccountSpecReq rq, CancellationToken ct)
    {
        var deletedCount = await _dbContext.AccountSpecs
            .Where(a => a.Id == rq.Id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        return deletedCount > 0;
    }
}
