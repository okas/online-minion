using ErrorOr;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Shared.Handlers;

internal abstract class BaseDeleteModelReqHlr<TRequest, TEntity>(IOnlineMinionDbContext dbContext)
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
