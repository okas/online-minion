using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpec.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.Shared;

namespace OnlineMinion.Application.PaymentSpec.Handlers;

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
