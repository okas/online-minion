using System.Linq.Expressions;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecsReqHlr : BaseQueryHandler<BaseGetSomePagedReq<AccountSpecResp>, AccountSpecResp>,
    IRequestHandler<BaseGetSomePagedReq<AccountSpecResp>, PagedResult<AccountSpecResp>>
{
    public GetAccountSpecsReqHlr(OnlineMinionDbContext dbContext) : base(dbContext) { }

    public async Task<PagedResult<AccountSpecResp>> Handle(
        BaseGetSomePagedReq<AccountSpecResp> rq,
        CancellationToken                    ct
    )
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
