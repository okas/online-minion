using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Responses;
using OnlineMinion.Data;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecsReqHlr(OnlineMinionDbContext dbContext)
    : BaseGetSomeModelsPagedReqHlr<Data.Entities.AccountSpec, AccountSpecResp>(dbContext)
{
    protected override Expression<Func<Data.Entities.AccountSpec, AccountSpecResp>> Projection =>
        e => new(
            e.Id,
            e.Name,
            e.Group,
            e.Description
        );
}
