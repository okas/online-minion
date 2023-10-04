using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecCashHandlers;

[UsedImplicitly]
internal sealed class CreatePaymentSpecCashReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreatePaymentSpecCashReq, PaymentSpecCash>(dbContext)
{
    protected override PaymentSpecCash ToEntity(CreatePaymentSpecCashReq rq) => new(
        rq.Name,
        rq.CurrencyCode,
        rq.Tags
    );
}
