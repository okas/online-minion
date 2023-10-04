using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecSharedHandlers;

[UsedImplicitly]
internal sealed class DeletePaymentSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseDeleteModelReqHlr<DeletePaymentSpecReq, PaymentSpecCash, PaymentSpecId>(dbContext)
{
    protected override PaymentSpecId CreateEntityId(DeletePaymentSpecReq rq) => new(rq.Id);
}
