using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecsReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetSomeModelsPagedReqHlr<Domain.AccountSpec, AccountSpecResp>(dbContext)
{
    protected override Expression<Func<Domain.AccountSpec, AccountSpecResp>> Projection =>
        e => new(
            e.Id,
            e.Name,
            e.Group,
            e.Description
        );
}
