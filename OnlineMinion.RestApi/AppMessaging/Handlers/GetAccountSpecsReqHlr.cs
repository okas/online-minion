using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Helpers;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

public sealed class GetAccountSpecsReqHlr : IRequestHandler<GetAccountSpecsReq, PagedResult<AccountSpecResp>>
{
    private readonly OnlineMinionDbContext _dbContext;
    public GetAccountSpecsReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<PagedResult<AccountSpecResp>> Handle(GetAccountSpecsReq rq, CancellationToken ct)
    {
        var query = _dbContext.AccountSpecs.AsNoTracking();

        // TODO: Filtering here

        // Default ordering.
        query = query.OrderBy(e => e.Id);

        var projectedQuery = query.Select(
            e => new AccountSpecResp(e.Id, e.Name, e.Group, e.Description)
        );

        var result = await projectedQuery.CreatePagedResultAsync(rq, ct);

        // TODO: "Delayed" Shouldn't  be used in production code, or should be able to opt-in from config.
        return result with
        {
            Result = result.Result.ToDelayedAsyncEnumerable(20, ct),
        };
    }
}
