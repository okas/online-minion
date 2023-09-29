using JetBrains.Annotations;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Domain.Shared;

namespace OnlineMinion.Application.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CreatePaymentSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreatePaymentSpecReq, BasePaymentSpec>(dbContext)
{
    protected override BasePaymentSpec ToEntity(CreatePaymentSpecReq rq) => new()
    {
        Name = rq.Name,
        CurrencyCode = rq.CurrencyCode,
        Tags = rq.Tags,
    };
}
