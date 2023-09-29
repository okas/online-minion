using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.Application.Contracts.AccountSpec.Responses;
using OnlineMinion.Application.Shared.Handlers;

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
