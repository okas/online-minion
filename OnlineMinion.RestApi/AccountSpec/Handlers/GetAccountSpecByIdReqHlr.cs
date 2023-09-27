using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.DataStore;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecByIdReqHlr(OnlineMinionDbContext dbContext)
    : BaseGetModelByIdReqHlr<GetAccountSpecByIdReq, Domain.AccountSpec, AccountSpecResp>(dbContext)
{
    protected override AccountSpecResp ToResponse(Domain.AccountSpec entity) => new()
    {
        Id = entity.Id,
        Group = entity.Group,
        Name = entity.Name,
        Description = entity.Description,
    };
}
