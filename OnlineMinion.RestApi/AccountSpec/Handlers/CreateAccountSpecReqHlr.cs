using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CreateAccountSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreateAccountSpecReq, Domain.AccountSpec>(dbContext)
{
    protected override Domain.AccountSpec ToEntity(CreateAccountSpecReq rq) =>
        new(rq.Name, rq.Group, rq.Description);
}
