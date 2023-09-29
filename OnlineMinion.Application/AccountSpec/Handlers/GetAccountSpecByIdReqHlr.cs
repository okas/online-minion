using JetBrains.Annotations;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Contracts.AccountSpec.Responses;

namespace OnlineMinion.Application.AccountSpec.Handlers;

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
