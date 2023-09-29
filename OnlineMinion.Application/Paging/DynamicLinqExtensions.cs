using System.Linq.Dynamic.Core;
using OnlineMinion.Contracts;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Paging;

public static class DynamicLinqExtensions
{
    public static IQueryable<TEntity> ConfigureStoreQuery<TEntity>(
        this IQueryable<TEntity> query,
        IFullQueryParams         fullQueryParams
    ) where TEntity : BaseEntity
    {
        if (!string.IsNullOrWhiteSpace(fullQueryParams.Filter))
        {
            query = query.Where(fullQueryParams.Filter);
        }

        query = string.IsNullOrWhiteSpace(fullQueryParams.Sort)
            ? query.OrderBy(e => e.Id)
            : query.OrderBy(fullQueryParams.Sort);

        return query.Skip((fullQueryParams.Page - 1) * fullQueryParams.Size).Take(fullQueryParams.Size);
    }
}
