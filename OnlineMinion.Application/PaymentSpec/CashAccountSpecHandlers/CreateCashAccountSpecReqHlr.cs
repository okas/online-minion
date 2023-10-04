using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpec.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpec.CashAccountSpecHandlers;

[UsedImplicitly]
internal sealed class CreateCashAccountSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreatePaymentSpecReq, CashAccountSpec>(dbContext)
{
    protected override CashAccountSpec ToEntity(CreatePaymentSpecReq rq) => new(
        rq.Name,
        rq.CurrencyCode,
        rq.Tags
    );
}
