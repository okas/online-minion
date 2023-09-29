using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.Application.Shared.Handlers;

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
