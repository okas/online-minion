using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

internal sealed class DeleteAccountSpecReqHlr : IRequestHandler<DeleteAccountSpecReq, ErrorOr<Deleted>>
{
    private readonly OnlineMinionDbContext _dbContext;
    public DeleteAccountSpecReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<ErrorOr<Deleted>> Handle(DeleteAccountSpecReq rq, CancellationToken ct)
    {
        var deletedCount = await _dbContext.AccountSpecs
            .Where(a => a.Id == rq.Id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        return deletedCount > 0 ? Result.Deleted : Error.NotFound();
    }
}
