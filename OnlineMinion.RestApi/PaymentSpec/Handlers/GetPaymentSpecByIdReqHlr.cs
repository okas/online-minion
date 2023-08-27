using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.Data;
using OnlineMinion.Data.Entities.Shared;
using OnlineMinion.RestApi.Shared.Handlers;

namespace OnlineMinion.RestApi.PaymentSpec.Handlers;

[UsedImplicitly]
internal class GetPaymentSpecByIdReqHlr(OnlineMinionDbContext dbContext)
    : BaseGetModelByIdReqHlr<GetPaymentSpecByIdReq, BasePaymentSpec, PaymentSpecResp?>(dbContext)
{
    protected override PaymentSpecResp? ToResponse(BasePaymentSpec entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        CurrencyCode = entity.CurrencyCode,
        Tags = entity.Tags,
    };
}
