using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.Application.Contracts.AccountSpec.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.AccountSpecs;

namespace OnlineMinion.Application.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecByIdReqHlr(IOnlineMinionDbContext dbContext) : BaseGetModelByIdReqHlr<
    GetAccountSpecByIdReq,
    Domain.AccountSpecs.AccountSpec,
    AccountSpecId,
    AccountSpecResp
>(dbContext)
{
    protected override AccountSpecId CreateEntityId(GetAccountSpecByIdReq rq) => new(rq.Id);

    protected override AccountSpecResp ToResponse(Domain.AccountSpecs.AccountSpec entity) => new()
    {
        Id = entity.Id.Value,
        Group = entity.Group,
        Name = entity.Name,
        Description = entity.Description,
    };
}
