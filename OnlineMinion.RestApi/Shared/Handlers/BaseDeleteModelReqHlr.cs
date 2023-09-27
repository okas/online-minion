using ErrorOr;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.DataStore;
using OnlineMinion.Domain;

namespace OnlineMinion.RestApi.Shared.Handlers;

internal abstract class BaseDeleteModelReqHlr<TRequest, TEntity>(OnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<TRequest, Deleted>
    where TRequest : IDeleteByIdCommand
    where TEntity : BaseEntity
{
    public async Task<ErrorOr<Deleted>> Handle(TRequest rq, CancellationToken ct)
    {
        var deletedCount = await dbContext.Set<TEntity>()
            .Where(e => e.Id == rq.Id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        return deletedCount > 0 ? Result.Deleted : Error.NotFound();
    }
}
