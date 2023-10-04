using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpec.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpec.CommonHandlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecDescriptorsReqHlr(IOnlineMinionDbContext dbContext)
    : GetSomeModelDescriptorsReqHlr<CashAccountSpec, PaymentSpecDescriptorResp>(dbContext)
{
    protected override Expression<Func<CashAccountSpec, PaymentSpecDescriptorResp>> Projection =>
        e => new(
            e.Id.Value,
            e.Name,
            e.CurrencyCode
        );
}
