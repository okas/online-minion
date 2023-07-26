using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities;

namespace OnlineMinion.RestApi.AppMessaging.Handlers;

internal sealed class GetAccountSpecByIdReqHlr : IRequestHandler<GetAccountSpecByIdReq, AccountSpecResp?>
{
    private readonly IQueryable<AccountSpec> _queryable;

    public GetAccountSpecByIdReqHlr(OnlineMinionDbContext dbContext) =>
        _queryable = dbContext.AccountSpecs.AsNoTracking();

    public async Task<AccountSpecResp?> Handle(GetAccountSpecByIdReq rq, CancellationToken ct)
    {
        var task = _queryable.FirstOrDefaultAsync(a => a.Id == rq.Id, ct).ConfigureAwait(false);

        if (await task is not { } entity)
        {
            return null;
        }

        return new()
        {
            Id = entity.Id,
            Group = entity.Group,
            Name = entity.Name,
            Description = entity.Description,
        };
    }
}
