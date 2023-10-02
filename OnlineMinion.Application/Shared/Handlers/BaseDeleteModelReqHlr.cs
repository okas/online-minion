using ErrorOr;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Shared.Handlers;

internal abstract class BaseDeleteModelReqHlr<TRequest, TEntity, TId>(IOnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<TRequest, Deleted>
    where TRequest : IDeleteByIdCommand
    where TEntity : class, IEntity<TId>
    where TId : class, IId
{
    public async Task<ErrorOr<Deleted>> Handle(TRequest rq, CancellationToken ct)
    {
        var id = CreateEntityId(rq);

        var deletedCount = await dbContext.Set<TEntity>()
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync(ct)
            .ConfigureAwait(false);

        return deletedCount > 0 ? Result.Deleted : Error.NotFound();
    }

    protected abstract TId CreateEntityId(TRequest rq);
}
