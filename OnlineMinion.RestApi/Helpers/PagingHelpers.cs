using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;

namespace OnlineMinion.RestApi.Helpers;

public static class PagingHelpers
{
    [Pure]
    public static async ValueTask<PagingMetaInfo> CreatePagingMetaAsync<TSource>(
        IQueryable<TSource> query,
        IPagingInfo         pagingInfo,
        CancellationToken   ct
    ) => new(await query.CountAsync(ct).ConfigureAwait(false), pagingInfo.Size, pagingInfo.Page);
}
