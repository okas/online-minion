using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.DataStore;
using OnlineMinion.Domain.Shared;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CreatePaymentSpecReqHlr(OnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreatePaymentSpecReq, BasePaymentSpec>(dbContext)
{
    protected override BasePaymentSpec ToEntity(CreatePaymentSpecReq rq) => new()
    {
        Name = rq.Name,
        CurrencyCode = rq.CurrencyCode,
        Tags = rq.Tags,
    };
}
