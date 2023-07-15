using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;
using OnlineMinion.RestApi.Helpers;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class GetAccountSpecsReqHlr : IRequestHandler<GetAccountSpecsReq, BasePagedResult<AccountSpecResp>>
{
    private readonly IQueryable<AccountSpec> _queryable;

    public GetAccountSpecsReqHlr(OnlineMinionDbContext dbContext) => _queryable = dbContext.AccountSpecs.AsNoTracking();

    public async Task<BasePagedResult<AccountSpecResp>> Handle(GetAccountSpecsReq rq, CancellationToken ct)
    {
        var pagingMeta = await PagingHelpers.CreateFromQueryable(_queryable, rq, ct).ConfigureAwait(false);

        var entities = _queryable
            .OrderBy(e => e.Id)
            .Skip(pagingMeta.ItemsOffset)
            .Take(pagingMeta.Size)
            .Select(e => new AccountSpecResp(e.Id, e.Name, e.Group, e.Description))
            .AsAsyncEnumerable();

        // TODO: Shouldn't  be used in production code, or should be able to opt-in from config.
        return new(Paging: pagingMeta, Result: entities.ToDelayedAsyncEnumerable(20, ct));
    }
}
