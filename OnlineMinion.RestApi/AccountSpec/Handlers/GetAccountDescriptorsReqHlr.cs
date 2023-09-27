using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.DataStore;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountDescriptorsReqHlr(OnlineMinionDbContext dbContext)
    : GetSomeModelDescriptorsReqHlr<Domain.AccountSpec, AccountSpecDescriptorResp>(dbContext)
{
    protected override Expression<Func<Domain.AccountSpec, AccountSpecDescriptorResp>> Projection =>
        e => new(
            e.Id,
            e.Name
        );
}
