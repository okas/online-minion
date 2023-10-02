using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Shared.Handlers;

internal abstract class BaseUpdateModelReqHlr<TRequest, TEntity, TId>(IOnlineMinionDbContext dbContext, ILogger logger)
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
            logger.LogWarning("{ModelName} with Id {Id} not found", typeof(TEntity).Name, rq.Id);

            return Error.NotFound();
        }

        UpdateEntity(entity, rq);

        return default;
    }

    protected abstract TId CreateEntityId(TRequest rq);

    protected abstract void UpdateEntity(TEntity entity, TRequest rq);
}
