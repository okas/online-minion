using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Domain.Shared;

namespace OnlineMinion.Application.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecReqHlr(IOnlineMinionDbContext dbContext, ILogger<UpdatePaymentSpecReqHlr> logger)
    : BaseUpdateModelReqHlr<UpdatePaymentSpecReq, BasePaymentSpec>(dbContext, logger)
{
    protected override void UpdateEntityAsync(BasePaymentSpec entity, UpdatePaymentSpecReq rq)
    {
        entity.Name = rq.Name;
        entity.CurrencyCode = rq.CurrencyCode;
        entity.Tags = rq.Tags;
    }
}
