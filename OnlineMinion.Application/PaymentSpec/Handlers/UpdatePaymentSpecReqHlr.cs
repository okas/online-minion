using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpec.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseUpdateModelReqHlr<UpdatePaymentSpecReq, CashAccountSpec, PaymentSpecId>(dbContext)
{
    protected override PaymentSpecId CreateEntityId(UpdatePaymentSpecReq rq) => new(rq.Id);

    protected override void UpdateEntity(CashAccountSpec entity, UpdatePaymentSpecReq rq) =>
        entity.Update(rq.Name, rq.Tags);
}
