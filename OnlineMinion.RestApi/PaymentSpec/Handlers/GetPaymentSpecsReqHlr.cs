using System.Linq.Expressions;
using JetBrains.Annotations;
using OnlineMinion.Application;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Domain.Shared;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecsReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetSomeModelsPagedReqHlr<BasePaymentSpec, PaymentSpecResp>(dbContext)
{
    protected override Expression<Func<BasePaymentSpec, PaymentSpecResp>> Projection =>
        e => new(e.Id, e.Name, e.CurrencyCode, e.Tags);
}
