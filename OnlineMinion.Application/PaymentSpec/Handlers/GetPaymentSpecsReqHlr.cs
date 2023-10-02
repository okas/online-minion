using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpec.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecsReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetSomeModelsPagedReqHlr<CashAccountSpec, PaymentSpecResp>(dbContext)
{
    protected override Expression<Func<CashAccountSpec, PaymentSpecResp>> Projection =>
        e => new(e.Id.Value, e.Name, e.CurrencyCode, e.Tags);
}
