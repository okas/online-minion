using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.AccountSpecs;

namespace OnlineMinion.Application.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdateAccountSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseUpdateModelReqHlr<UpdateAccountSpecReq, Domain.AccountSpecs.AccountSpec, AccountSpecId>(dbContext)
{
    protected override AccountSpecId CreateEntityId(UpdateAccountSpecReq rq) => new(rq.Id);

    protected override void UpdateEntity(Domain.AccountSpecs.AccountSpec entity, UpdateAccountSpecReq rq)
    {
        entity.Name = rq.Name;
        entity.Group = rq.Group;
        entity.Description = rq.Description;
    }
}
