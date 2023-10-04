using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpec.CashAccountSpecHandlers;

[UsedImplicitly]
internal sealed class CreateCashAccountSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreatePaymentSpecCashReq, PaymentSpecCash>(dbContext)
{
    protected override PaymentSpecCash ToEntity(CreatePaymentSpecCashReq rq) => new(
        rq.Name,
        rq.CurrencyCode,
        rq.Tags
    );
}
