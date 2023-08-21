using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

internal sealed class GetAccountSpecsReqHlr : BaseQueryHandler<BaseGetSomeReq<AccountSpecResp>, AccountSpecResp>,
    IRequestHandler<BaseGetSomeReq<AccountSpecResp>, PagedResult<AccountSpecResp>>
{
    public GetAccountSpecsReqHlr(OnlineMinionDbContext dbContext) : base(dbContext) { }

    [Pure]
    public async Task<PagedResult<AccountSpecResp>> Handle(BaseGetSomeReq<AccountSpecResp> rq, CancellationToken ct)
    {
        var query = DbContext.AccountSpecs.AsNoTracking();

        Expression<Func<Data.Entities.AccountSpec, AccountSpecResp>> projection = e => new(
            e.Id,
            e.Name,
            e.Group,
            e.Description
        );

        var result = await GetDataFromStoreAsync(query, rq, projection, ct).ConfigureAwait(false);

        // TODO: "Delayed" Shouldn't  be used in production code, or should be able to opt-in from config.
        return result with
        {
            Result = result.Result.ToDelayedAsyncEnumerable(20, ct),
        };
    }
}
