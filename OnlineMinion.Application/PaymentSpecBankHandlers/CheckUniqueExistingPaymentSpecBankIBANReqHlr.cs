using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecBankHandlers;

/// <summary>Check that Bank Payment Spec must have a unique IBAN for existing instance.</summary>
/// <inheritdoc cref="BaseCheckUniqueModelReqHlr{TRequest,TEntity}" />
[UsedImplicitly]
internal sealed class CheckUniqueExistingPaymentSpecBankIBANReqHlr(IServiceScopeFactory scopeFactory)
    : BaseCheckUniqueModelReqHlr<CheckUniqueExistingPaymentSpecBankIBANReq, PaymentSpecBank>(scopeFactory)
{
    protected override Expression<Func<PaymentSpecBank, bool>> GetConflictPredicate(
        CheckUniqueExistingPaymentSpecBankIBANReq rq
    ) =>
        entity => entity.IBAN == rq.MemberValue
                  && entity.Id != new PaymentSpecId(rq.OwnId);
}
