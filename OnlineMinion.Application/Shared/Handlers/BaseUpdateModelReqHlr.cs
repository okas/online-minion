using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Shared.Handlers;

internal abstract class BaseUpdateModelReqHlr<TRequest, TEntity>(IOnlineMinionDbContext dbContext, ILogger logger)
    : IRequestHandler<TRequest, ErrorOr<Updated>>
    where TRequest : IUpdateCommand
    where TEntity : BaseEntity
{
    public async Task<ErrorOr<Updated>> Handle(TRequest rq, CancellationToken ct)
    {
        var entity = await dbContext.Set<TEntity>()
            .FindAsync(new object?[] { rq.Id, }, ct)
            .ConfigureAwait(false);

        if (entity is null)
        {
            logger.LogWarning("{ModelName} with Id {Id} not found", typeof(TEntity).Name, rq.Id);

            return Error.NotFound();
        }

        UpdateEntityAsync(entity, rq);

        return default;
    }

    protected abstract void UpdateEntityAsync(TEntity entity, TRequest rq);
}
