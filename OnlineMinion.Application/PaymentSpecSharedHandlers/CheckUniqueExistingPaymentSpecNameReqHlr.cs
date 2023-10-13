using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecSharedHandlers;

/// <summary>
///     Check the business rule, that the existing payment spec, despite the type, must have a unique name.
///     Therefore the entity type is <see cref="BasePaymentSpecData" /> and not <see cref="PaymentSpecBank" />.
///     It means that the rule is applied to all payment spec types.
/// </summary>
/// <inheritdoc cref="BaseCheckUniqueModelReqHlr{TRequest,TEntity}" />
[UsedImplicitly]
internal sealed class CheckUniqueExistingPaymentSpecNameReqHlr(IServiceScopeFactory scopeFactory)
    : BaseCheckUniqueModelReqHlr<CheckUniqueExistingPaymentSpecNameReq, BasePaymentSpecData>(scopeFactory)
{
    protected override Expression<Func<BasePaymentSpecData, bool>> GetConflictPredicate(
        CheckUniqueExistingPaymentSpecNameReq rq
    ) =>
        entity => entity.Name == rq.MemberValue
                  && entity.Id != new PaymentSpecId(rq.OwnId);
}
