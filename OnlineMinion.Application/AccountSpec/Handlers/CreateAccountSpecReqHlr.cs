using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.Application.Shared.Handlers;

namespace OnlineMinion.Application.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CreateAccountSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreateAccountSpecReq, Domain.AccountSpecs.AccountSpec>(dbContext)
{
    protected override Domain.AccountSpecs.AccountSpec ToEntity(CreateAccountSpecReq rq) =>
        new(rq.Name, rq.Group, rq.Description);
}
