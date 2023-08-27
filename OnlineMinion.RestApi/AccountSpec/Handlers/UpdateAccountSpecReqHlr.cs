using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdateAccountSpecReqHlr(OnlineMinionDbContext dbContext, ILogger<UpdateAccountSpecReqHlr> logger)
    : BaseUpdateModelReqHlr<UpdateAccountSpecReq, Data.Entities.AccountSpec>(dbContext, logger)
{
    protected override void UpdateEntityAsync(Data.Entities.AccountSpec entity, UpdateAccountSpecReq rq)
    {
        entity.Name = rq.Name;
        entity.Group = rq.Group;
        entity.Description = rq.Description;
    }
}
