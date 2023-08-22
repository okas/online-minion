using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.RestApi.Helpers;

namespace OnlineMinion.RestApi.Handlers;

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
        var pagingMeta = await PagingHelpers.CreatePagingMetaAsync(query, queryParams, ct).ConfigureAwait(false);

        var resultStream = QueryHelpers
            .ConfigureStoreQuery(queryParams, query)
            .Select(projection)
            .AsAsyncEnumerable();

        return new(resultStream, pagingMeta);
    }
}