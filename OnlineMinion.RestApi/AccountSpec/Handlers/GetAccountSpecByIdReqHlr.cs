using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecByIdReqHlr(OnlineMinionDbContext dbContext)
    : BaseGetModelByIdReqHlr<GetAccountSpecByIdReq, Data.Entities.AccountSpec, AccountSpecResp>(dbContext)
{
    protected override AccountSpecResp ToResponse(Data.Entities.AccountSpec entity) => new()
    {
        Id = entity.Id,
        Group = entity.Group,
        Name = entity.Name,
        Description = entity.Description,
    };
}
