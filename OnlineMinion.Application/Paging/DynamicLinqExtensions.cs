using System.Linq.Dynamic.Core;
using OnlineMinion.Application.Contracts;
using OnlineMinion.Domain;

namespace OnlineMinion.Application.Paging;

public static class DynamicLinqExtensions
{
    public static IQueryable<TEntity> ConfigureStoreQuery<TEntity>(
        this IQueryable<TEntity> query,
        IQueryParameters         queryParams
    )
        where TEntity : IEntity<IId>
    {
        if (!string.IsNullOrWhiteSpace(queryParams.Filter))
        {
            query = query.Where(queryParams.Filter);
        }

        // TODO: default order by CreatedAt field based, need interface for entity and here to compare it´s compatibility
        query = string.IsNullOrWhiteSpace(queryParams.Sort)
            ? query.OrderBy(e => e.Id)
            : query.OrderBy(queryParams.Sort);

        return query.Skip((queryParams.Page - 1) * queryParams.Size).Take(queryParams.Size);
    }
}
