using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Queries;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;

namespace OnlineMinion.Common.CQRS.QueryHandlers;

public sealed class GetAccountSpecsQryHlr : IRequestHandler<GetAccountSpecsQry, BasePagedResult<AccountSpecResp>>
{
    private readonly IQueryable<AccountSpec> _queryable;

    public GetAccountSpecsQryHlr(OnlineMinionDbContext dbContext) => _queryable = dbContext.AccountSpecs.AsNoTracking();

    public async Task<BasePagedResult<AccountSpecResp>> Handle(GetAccountSpecsQry rq, CancellationToken ct)
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
