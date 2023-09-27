using System.Linq.Expressions;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;
using OnlineMinion.Data;
using OnlineMinion.Domain;
using OnlineMinion.RestApi.Helpers;
using OnlineMinion.RestApi.Paging;

namespace OnlineMinion.RestApi.Shared.Handlers;

internal abstract class BaseGetSomeModelsPagedReqHlr<TEntity, TResponse>(OnlineMinionDbContext dbContext)
    : IErrorOrRequestHandler<GetSomeModelsPagedReq<TResponse>, PagedStreamResult<TResponse>>
    where TEntity : BaseEntity
{
    protected abstract Expression<Func<TEntity, TResponse>> Projection { get; }

    public async Task<ErrorOr<PagedStreamResult<TResponse>>> Handle(
        GetSomeModelsPagedReq<TResponse> rq,
        CancellationToken                ct
    )
    {
        var query = dbContext.Set<TEntity>().AsNoTracking();

        query = SetIncludes(query);

        var pagingMeta = await query.GetPagingMetaInfoAsync(rq, ct).ConfigureAwait(false);

        var resultStream = query
            .ConfigureStoreQuery(rq)
            .Select(Projection)
            .AsAsyncEnumerable();

        return new PagedStreamResult<TResponse>(resultStream, pagingMeta);
    }

    protected virtual IQueryable<TEntity> SetIncludes(IQueryable<TEntity> query) => query;
}
