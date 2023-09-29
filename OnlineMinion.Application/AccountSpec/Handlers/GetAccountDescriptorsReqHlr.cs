using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Responses;
using OnlineMinion.Application.Shared.Handlers;

namespace OnlineMinion.Application.AccountSpec.Handlers;

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
