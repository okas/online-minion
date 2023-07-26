using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

internal sealed class GetAccountSpecsReqHlr : BaseQueryHandler<GetAccountSpecsReq, AccountSpecResp>,
    IRequestHandler<GetAccountSpecsReq, PagedResult<AccountSpecResp>>
{
    public GetAccountSpecsReqHlr(OnlineMinionDbContext dbContext) : base(dbContext) { }

    [Pure]
    public async Task<PagedResult<AccountSpecResp>> Handle(GetAccountSpecsReq rq, CancellationToken ct)
    {
        var query = DbContext.AccountSpecs.AsNoTracking();

        Expression<Func<AccountSpec, AccountSpecResp>> projection = e => new(e.Id, e.Name, e.Group, e.Description);

        var result = await GetDataFromStoreAsync(query, rq, projection, ct);

        // TODO: "Delayed" Shouldn't  be used in production code, or should be able to opt-in from config.
        return result with
        {
            Result = result.Result.ToDelayedAsyncEnumerable(20, ct),
        };
    }
}
