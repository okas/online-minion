using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecCashHandlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecCashReqHlr(IOnlineMinionDbContext dbContext)
    : BaseUpdateModelReqHlr<UpdatePaymentSpecCashReq, PaymentSpecCash, PaymentSpecId>(dbContext)
{
    protected override PaymentSpecId CreateEntityId(UpdatePaymentSpecCashReq rq) => new(rq.Id);

    protected override void UpdateEntity(PaymentSpecCash entity, UpdatePaymentSpecCashReq rq) =>
        entity.Update(rq.Name, rq.Tags);
}
