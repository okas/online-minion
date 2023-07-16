using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AppMessaging;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.RestApi.Helpers;

internal static class PagedResultExtensions
{
    [Pure]
    public static async ValueTask<PagedResult<TModel>> CreatePagedResultAsync<TModel>(
        this IQueryable<TModel> query,
        IPagedRequest           pagedRequest,
        CancellationToken       ct
    )
    {
        var pagingMeta = await PagingHelpers.CreateFromQueryableAsync(query, pagedRequest, ct).ConfigureAwait(false);

        query = query.Skip(pagingMeta.ItemsOffset).Take(pagingMeta.Size);

        var resultStream = query.AsAsyncEnumerable();

        return new(resultStream, pagingMeta);
    }
}
