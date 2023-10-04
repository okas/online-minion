using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.PaymentSpec.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecReqHlr(IOnlineMinionDbContext dbContext, ILogger<UpdatePaymentSpecReqHlr> logger)
    : BaseUpdateModelReqHlr<UpdatePaymentSpecReq, CashAccountSpec, PaymentSpecId>(dbContext, logger)
{
    protected override PaymentSpecId CreateEntityId(UpdatePaymentSpecReq rq) => new(rq.Id);

    protected override void UpdateEntity(CashAccountSpec entity, UpdatePaymentSpecReq rq)
    {
        entity.Name = rq.Name;
        entity.Tags = rq.Tags;
    }
}
