using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountDescriptorsReqHlr(IOnlineMinionDbContext dbContext)
    : GetSomeModelDescriptorsReqHlr<Domain.AccountSpec, AccountSpecDescriptorResp>(dbContext)
{
    protected override Expression<Func<Domain.AccountSpec, AccountSpecDescriptorResp>> Projection =>
        e => new(
            e.Id,
            e.Name
        );
}
