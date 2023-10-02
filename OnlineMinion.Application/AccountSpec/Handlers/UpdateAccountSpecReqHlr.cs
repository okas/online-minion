using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.AccountSpecs;

namespace OnlineMinion.Application.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdateAccountSpecReqHlr(IOnlineMinionDbContext dbContext, ILogger<UpdateAccountSpecReqHlr> logger)
    : BaseUpdateModelReqHlr<UpdateAccountSpecReq, Domain.AccountSpecs.AccountSpec, AccountSpecId>(dbContext, logger)
{
    protected override AccountSpecId CreateEntityId(UpdateAccountSpecReq rq) => new(rq.Id);

    protected override void UpdateEntity(Domain.AccountSpecs.AccountSpec entity, UpdateAccountSpecReq rq)
    {
        entity.Name = rq.Name;
        entity.Group = rq.Group;
        entity.Description = rq.Description;
    }
}
