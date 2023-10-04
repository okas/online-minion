using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecSharedHandlers;

[UsedImplicitly]
internal sealed class GetSomePaymentSpecCashReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetSomeModelsPagedReqHlr<PaymentSpecCash, PaymentSpecResp>(dbContext)
{
    protected override Expression<Func<PaymentSpecCash, PaymentSpecResp>> Projection =>
        e => new(e.Id.Value, e.Name, e.CurrencyCode, e.Tags, PaymentSpecType.Cash);
}
