using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities.Shared;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecDescriptorsReqHlr(OnlineMinionDbContext dbContext)
    : GetSomeModelDescriptorsReqHlr<BasePaymentSpec, PaymentSpecDescriptorResp>(dbContext)
{
    protected override Expression<Func<BasePaymentSpec, PaymentSpecDescriptorResp>> Projection =>
        e => new(
            e.Id,
            e.Name,
            e.CurrencyCode
        );
}