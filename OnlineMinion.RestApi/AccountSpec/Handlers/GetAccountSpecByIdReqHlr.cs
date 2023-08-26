using ErrorOr;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using OnlineMinion.Common;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Data;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecByIdReqHlr : IApiRequestHandler<GetAccountSpecByIdReq, AccountSpecResp?>
{
    private readonly OnlineMinionDbContext _dbContext;
    public GetAccountSpecByIdReqHlr(OnlineMinionDbContext dbContext) => _dbContext = dbContext;

    public async Task<ErrorOr<AccountSpecResp?>> Handle(GetAccountSpecByIdReq rq, CancellationToken ct)
    {
        var entity = await _dbContext.AccountSpecs.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == rq.Id, ct)
            .ConfigureAwait(false);

        if (entity is null)
        {
            return default;
        }

        return new AccountSpecResp
        {
            Id = entity.Id,
            Group = entity.Group,
            Name = entity.Name,
            Description = entity.Description,
        };
    }
}
