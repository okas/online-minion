using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Responses;
using OnlineMinion.Application.Shared.Handlers;

namespace OnlineMinion.Application.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class GetAccountSpecsReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetSomeModelsPagedReqHlr<Domain.AccountSpecs.AccountSpec, AccountSpecResp>(dbContext)
{
    protected override Expression<Func<Domain.AccountSpecs.AccountSpec, AccountSpecResp>> Projection =>
        e => new(
            e.Id.Value,
            e.Name,
            e.Group,
            e.Description
        );
}
