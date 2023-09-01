using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountDescriptorsReqHlr(OnlineMinionDbContext dbContext)
    : GetSomeModelDescriptorsReqHlr<Data.Entities.AccountSpec, AccountSpecDescriptorResp>(dbContext)
{
    protected override Expression<Func<Data.Entities.AccountSpec, AccountSpecDescriptorResp>> Projection =>
        e => new(
            e.Id,
            e.Name
        );
}
