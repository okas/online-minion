using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Responses;
using OnlineMinion.Application.Shared.Handlers;

namespace OnlineMinion.Application.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountDescriptorsReqHlr(IOnlineMinionDbContext dbContext)
    : GetSomeModelDescriptorsReqHlr<Domain.AccountSpecs.AccountSpec, AccountSpecDescriptorResp>(dbContext)
{
    protected override Expression<Func<Domain.AccountSpecs.AccountSpec, AccountSpecDescriptorResp>> Projection =>
        e => new(
            e.Id.Value,
            e.Name
        );
}
