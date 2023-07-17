using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Responses;

namespace OnlineMinion.RestApi.Helpers;

internal static class PagedResultExtensions
{
    public static async ValueTask<PagedResult<TModel>> RetrieveDataAsync<TModel>(
        this IQueryable<TModel> query,
        IPagingInfo             pagingInfo,
        CancellationToken       ct
    )
    {
        var pagingMeta = await PagingHelpers.CreateFromQueryableAsync(query, pagingInfo, ct).ConfigureAwait(false);

        var resultStream = query.AsAsyncEnumerable();

        return new(resultStream, pagingMeta);
    }
}
