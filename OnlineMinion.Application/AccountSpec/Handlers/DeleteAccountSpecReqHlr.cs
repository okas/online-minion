using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.AccountSpecs;

namespace OnlineMinion.Application.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class DeleteAccountSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeleteAccountSpecReq, Domain.AccountSpecs.AccountSpec, AccountSpecId>(dbContext)
{
    protected override AccountSpecId CreateEntityId(DeleteAccountSpecReq rq) => new(rq.Id);
}
