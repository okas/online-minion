using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecByIdReqHlr(IOnlineMinionDbContext dbContext)
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
