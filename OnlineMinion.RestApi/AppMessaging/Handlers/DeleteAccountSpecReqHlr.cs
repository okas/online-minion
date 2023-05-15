using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class DeleteAccountSpecReqHlr : IRequestHandler<DeleteAccountSpecReq, bool>
{
    private readonly OnlineMinionDbContext _dbContext;

    public DeleteAccountSpecReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public Task<bool> Handle(DeleteAccountSpecReq rq, CancellationToken ct)
    {
        var task = _dbContext.AccountSpecs
            .Where(a => a.Id == rq.Id)
            .ExecuteDeleteAsync(ct);

        return Task.FromResult(task.Result > 0);
    }
}
