using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.Application.Shared.Handlers;

namespace OnlineMinion.Application.AccountSpec.Handlers;

/// <summary>Check that new Account Spec must have a unique name for new instance.</summary>
/// <inheritdoc cref="BaseCheckUniqueModelReqHlr{TRequest,TEntity}" />
[UsedImplicitly]
internal sealed class CheckUniqueNewAccountSpecReqHlr(IServiceScopeFactory scopeFactory)
    : BaseCheckUniqueModelReqHlr<CheckAccountSpecUniqueNewReq, Domain.AccountSpecs.AccountSpec>(scopeFactory)
{
    protected override Expression<Func<Domain.AccountSpecs.AccountSpec, bool>> GetConflictPredicate(
        CheckAccountSpecUniqueNewReq rq
    ) =>
        entity => entity.Name == rq.MemberValue;
}
