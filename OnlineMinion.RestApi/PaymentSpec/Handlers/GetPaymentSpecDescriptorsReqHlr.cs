using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Domain.Shared;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecDescriptorsReqHlr(IOnlineMinionDbContext dbContext)
    : GetSomeModelDescriptorsReqHlr<BasePaymentSpec, PaymentSpecDescriptorResp>(dbContext)
{
    protected override Expression<Func<BasePaymentSpec, PaymentSpecDescriptorResp>> Projection =>
        e => new(
            e.Id,
            e.Name,
            e.CurrencyCode
        );
}
