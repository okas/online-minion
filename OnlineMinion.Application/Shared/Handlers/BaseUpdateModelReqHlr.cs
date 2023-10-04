using ErrorOr;
using MediatR;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Shared.Handlers;

internal abstract class BaseUpdateModelReqHlr<TRequest, TEntity, TId>(IOnlineMinionDbContext dbContext)
    : IRequestHandler<TRequest, ErrorOr<Updated>>
    where TRequest : IUpdateCommand
    where TEntity : class, IEntity<TId>
    where TId : class, IId
{
    public async Task<ErrorOr<Updated>> Handle(TRequest rq, CancellationToken ct)
    {
        var id = CreateEntityId(rq);

        var entity = await dbContext.Set<TEntity>()
            .FindAsync(new object?[] { id, }, ct)
            .ConfigureAwait(false);

        if (entity is null)
        {
            return Error.NotFound(description: $"{typeof(TEntity).Name} with Id {rq.Id} not found");
        }

        UpdateEntity(entity, rq);

        return default;
    }

    protected abstract TId CreateEntityId(TRequest rq);

    protected abstract void UpdateEntity(TEntity entity, TRequest rq);
}
