using JetBrains.Annotations;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Application.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CreateAccountSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreateAccountSpecReq, Domain.AccountSpec>(dbContext)
{
    protected override Domain.AccountSpec ToEntity(CreateAccountSpecReq rq) =>
        new(rq.Name, rq.Group, rq.Description);
}
