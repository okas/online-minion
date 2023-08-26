using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Responses;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Helpers;

namespace OnlineMinion.RestApi.Shared.Handlers;

internal abstract class BaseQueryHandler<TRequest, TResponse>
    where TRequest : IQueryParams
{
    protected readonly OnlineMinionDbContext DbContext;
    protected BaseQueryHandler(OnlineMinionDbContext dbContext) => DbContext = dbContext;

    protected static async ValueTask<PagedResult<TResponse>> GetDataFromStoreAsync<TEntity>(
        IQueryable<TEntity>                  query,
        TRequest                             queryParams,
        Expression<Func<TEntity, TResponse>> projection,
        CancellationToken                    ct = default
    ) where TEntity : BaseEntity
    {
        var pagingMeta = await query.GetPagingMetaInfoAsync(queryParams, ct).ConfigureAwait(false);

        var resultStream = query.ConfigureStoreQuery(queryParams)
            .Select(projection)
            .AsAsyncEnumerable();

        return new(resultStream, pagingMeta);
    }
}
