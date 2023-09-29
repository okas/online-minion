using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Contracts.AccountSpec.Requests;

namespace OnlineMinion.Application.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdateAccountSpecReqHlr(IOnlineMinionDbContext dbContext, ILogger<UpdateAccountSpecReqHlr> logger)
    : BaseUpdateModelReqHlr<UpdateAccountSpecReq, Domain.AccountSpec>(dbContext, logger)
{
    protected override void UpdateEntityAsync(Domain.AccountSpec entity, UpdateAccountSpecReq rq)
    {
        entity.Name = rq.Name;
        entity.Group = rq.Group;
        entity.Description = rq.Description;
    }
}
