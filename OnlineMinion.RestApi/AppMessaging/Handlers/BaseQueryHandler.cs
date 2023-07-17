using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;
using OnlineMinion.Data;
using OnlineMinion.Data.BaseEntities;
using OnlineMinion.RestApi.Helpers;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

internal abstract class BaseQueryHandler<TRequest, TResponse>
    where TRequest : IQueryParams
{
    protected readonly OnlineMinionDbContext DbContext;
    protected BaseQueryHandler(OnlineMinionDbContext dbContext) => DbContext = dbContext;

    protected async Task<Contracts.Responses.PagedResult<TResponse>> GetDataFromStoreAsync<TEntity>(
        TRequest                             queryParams,
        Expression<Func<TEntity, TResponse>> projection,
        IQueryable<TEntity>?                 query = null,
        CancellationToken                    ct    = default
    ) where TEntity : BaseEntity
    {
        query = ConfigureStoreQuery(
            queryParams,
            query ?? DbContext.Set<TEntity>().AsNoTracking()
        );

        return await query.Select(projection).RetrieveDataAsync(queryParams, ct);
    }

    private static IQueryable<TEntity> ConfigureStoreQuery<TEntity>(TRequest queryParams, IQueryable<TEntity> query)
        where TEntity : BaseEntity
    {
        if (!string.IsNullOrWhiteSpace(queryParams.Filter))
        {
            query = query.Where(queryParams.Filter);
        }

        query = string.IsNullOrWhiteSpace(queryParams.Sort)
            ? query.OrderBy(e => e.Id)
            : query.OrderBy(queryParams.Sort);

        return query.Skip((queryParams.Page - 1) * queryParams.Size).Take(queryParams.Size);
    }
}
