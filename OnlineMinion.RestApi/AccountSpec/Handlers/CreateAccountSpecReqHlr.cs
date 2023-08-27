using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CreateAccountSpecReqHlr(OnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreateAccountSpecReq, Data.Entities.AccountSpec>(dbContext)
{
    protected override Data.Entities.AccountSpec ToEntity(CreateAccountSpecReq rq) =>
        new(rq.Name, rq.Group, rq.Description);
}
