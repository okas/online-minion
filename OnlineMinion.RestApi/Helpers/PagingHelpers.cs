using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AppMessaging;

namespace OnlineMinion.RestApi.Helpers;

public static class PagingHelpers
{
    public static async ValueTask<PagingMetaInfo> CreateFromQueryable<TSource>(
        IQueryable<TSource> query,
        IPagedRequest       rq,
        CancellationToken   ct
    ) => new(await query.CountAsync(ct).ConfigureAwait(false), rq.PageSize, rq.Page);
}
