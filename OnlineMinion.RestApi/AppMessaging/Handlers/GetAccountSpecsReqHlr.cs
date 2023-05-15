using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class GetAccountSpecsReqHlr : IRequestHandler<GetAccountSpecsReq, BasePagedResult<AccountSpecResp>>
{
    private readonly IQueryable<AccountSpec> _queryable;

    public GetAccountSpecsReqHlr(OnlineMinionDbContext dbContext) => _queryable = dbContext.AccountSpecs.AsNoTracking();

    public async Task<BasePagedResult<AccountSpecResp>> Handle(GetAccountSpecsReq rq, CancellationToken ct)
    {
        PagingMetaInfo pagingMeta = new(
            await _queryable.CountAsync(ct),
            rq.PageSize,
            rq.Page
        );

        var entities = await _queryable
            .OrderBy(e => e.Id)
            .Skip(pagingMeta.ItemsOffset)
            .Take(pagingMeta.Size)
            .Select(e => new AccountSpecResp(e.Id, e.Name, e.Group, e.Description))
            .ToListAsync(ct);

        return new(Paging: pagingMeta, Result: entities);
    }
}
