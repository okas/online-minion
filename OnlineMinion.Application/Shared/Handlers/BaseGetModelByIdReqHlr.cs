using ErrorOr;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Shared.Handlers;

internal abstract class BaseGetModelByIdReqHlr<TRequest, TEntity, TId, TResponse>(IOnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<TRequest, TResponse>
    where TRequest : IGetByIdRequest<TResponse>
    where TEntity : class, IEntity<TId>
    where TId : class, IId
{
    public async Task<ErrorOr<TResponse>> Handle(TRequest rq, CancellationToken ct)
    {
        var id = CreateEntityId(rq);

        var entity = await dbContext.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, ct)
            .ConfigureAwait(false);

        return entity is null ? Error.NotFound() : ToResponse(entity);
    }

    protected abstract TId CreateEntityId(TRequest rq);

    protected abstract TResponse ToResponse(TEntity entity);
}
