using System.Linq.Expressions;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Helpers;

namespace OnlineMinion.RestApi.Shared.Handlers;

internal abstract class BaseGetSomeModelsPagedReqHlr<TEntity, TResponse>(OnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<BaseGetSomeModelsPagedReq<TResponse>, PagedResult<TResponse>>
    where TEntity : BaseEntity
{
    protected abstract Expression<Func<TEntity, TResponse>> Projection { get; }

    public async Task<ErrorOr<PagedResult<TResponse>>> Handle(
        BaseGetSomeModelsPagedReq<TResponse> rq,
        CancellationToken                    ct
    )
    {
        var query = dbContext.Set<TEntity>().AsNoTracking();

        query = SetIncludes(query);

        var pagingMeta = await query.GetPagingMetaInfoAsync(rq, ct).ConfigureAwait(false);

        var resultStream = query
            .ConfigureStoreQuery(rq)
            .Select(Projection)
            .AsAsyncEnumerable();

        return new PagedResult<TResponse>(resultStream, pagingMeta);
    }

    protected virtual IQueryable<TEntity> SetIncludes(IQueryable<TEntity> query) => query;
}
