using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpec.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpec.CashAccountSpecHandlers;

[UsedImplicitly]
internal sealed class DeleteCashAccountSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeletePaymentSpecReq, CashAccountSpec, PaymentSpecId>(dbContext)
{
    protected override PaymentSpecId CreateEntityId(DeletePaymentSpecReq rq) => new(rq.Id);
}
