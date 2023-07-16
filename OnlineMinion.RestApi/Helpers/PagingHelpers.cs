using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AppMessaging;

namespace OnlineMinion.RestApi.Helpers;

public static class PagingHelpers
{
    [Pure]
    public static async ValueTask<PagingMetaInfo> CreateFromQueryableAsync<TSource>(
        IQueryable<TSource> query,
        IPagedRequest       rq,
        CancellationToken   ct
    ) => new(await query.CountAsync(ct).ConfigureAwait(false), rq.PageSize, rq.Page);
}
