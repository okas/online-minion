using MediatR;
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

    public async Task<PagedResult<AccountSpecResp>> Handle(GetAccountSpecsReq rq, CancellationToken ct)
    {
        var result = await GetDataFromStoreAsync<AccountSpec>(
            rq,
            e => new(e.Id, e.Name, e.Group, e.Description),
            ct: ct
        );

        // TODO: "Delayed" Shouldn't  be used in production code, or should be able to opt-in from config.
        return result with
        {
            Result = result.Result.ToDelayedAsyncEnumerable(20, ct),
        };
    }
}
