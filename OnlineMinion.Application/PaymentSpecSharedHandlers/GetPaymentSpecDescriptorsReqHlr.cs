using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecSharedHandlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecDescriptorsReqHlr(IOnlineMinionDbContext dbContext)
    : GetSomeModelDescriptorsReqHlr<PaymentSpecCash, PaymentSpecDescriptorResp>(dbContext)
{
    protected override Expression<Func<PaymentSpecCash, PaymentSpecDescriptorResp>> Projection =>
        e => new(
            e.Id.Value,
            e.Name,
            e.CurrencyCode
        );
}
