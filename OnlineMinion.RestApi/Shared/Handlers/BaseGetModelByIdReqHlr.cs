using ErrorOr;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Data;
using OnlineMinion.Domain;

namespace OnlineMinion.RestApi.Shared.Handlers;

internal abstract class BaseGetModelByIdReqHlr<TRequest, TEntity, TResponse>(OnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<TRequest, TResponse>
    where TRequest : IGetByIdRequest<TResponse>
    where TEntity : BaseEntity
{
    public async Task<ErrorOr<TResponse>> Handle(TRequest rq, CancellationToken ct)
    {
        var entity = await dbContext.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == rq.Id, ct)
            .ConfigureAwait(false);

        return entity is null ? Error.NotFound() : ToResponse(entity);
    }

    protected abstract TResponse ToResponse(TEntity entity);
}
