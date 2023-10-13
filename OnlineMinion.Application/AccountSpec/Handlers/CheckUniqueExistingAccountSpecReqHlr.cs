using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.AccountSpecs;

namespace OnlineMinion.Application.AccountSpec.Handlers;

/// <summary>Check that Account Spec must have a unique name for existing instance.</summary>
/// <inheritdoc cref="BaseCheckUniqueModelReqHlr{TRequest,TEntity}" />
[UsedImplicitly]
internal sealed class CheckUniqueExistingAccountSpecReqHlr(IServiceScopeFactory scopeFactory)
    : BaseCheckUniqueModelReqHlr<CheckAccountSpecUniqueExistingReq, Domain.AccountSpecs.AccountSpec>(scopeFactory)
{
    protected override Expression<Func<Domain.AccountSpecs.AccountSpec, bool>> GetConflictPredicate(
        CheckAccountSpecUniqueExistingReq rq
    ) =>
        entity => entity.Name == rq.MemberValue
                  && entity.Id != new AccountSpecId(rq.OwnId);
}
