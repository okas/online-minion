using System.Linq.Dynamic.Core;
using OnlineMinion.Contracts;
using OnlineMinion.Data.BaseEntities;

namespace OnlineMinion.RestApi.Helpers;

public static class QueryHelpers
{
    public static IQueryable<TEntity> ConfigureStoreQuery<TEntity>(IQueryParams queryParams, IQueryable<TEntity> query)
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