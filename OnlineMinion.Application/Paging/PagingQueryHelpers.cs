using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;

namespace OnlineMinion.Application.Paging;

public static class PagingQueryHelpers
{
    [Pure]
    public static async ValueTask<PagingMetaInfo> GetPagingMetaInfoAsync<TSource>(
        this IQueryable<TSource> query,
        IPagingInfo              pagingInfo,
        CancellationToken        ct
    ) => new(
        await query.CountAsync(ct).ConfigureAwait(false),
        pagingInfo.Size,
        pagingInfo.Page
    );
}
