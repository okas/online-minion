using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Data;
using OnlineMinion.Domain.Shared;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecReqHlr(OnlineMinionDbContext dbContext, ILogger<UpdatePaymentSpecReqHlr> logger)
    : BaseUpdateModelReqHlr<UpdatePaymentSpecReq, BasePaymentSpec>(dbContext, logger)
{
    protected override void UpdateEntityAsync(BasePaymentSpec entity, UpdatePaymentSpecReq rq)
    {
        entity.Name = rq.Name;
        entity.CurrencyCode = rq.CurrencyCode;
        entity.Tags = rq.Tags;
    }
}
