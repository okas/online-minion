using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpec.Requests;
using OnlineMinion.Application.Contracts.PaymentSpec.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.Shared;

namespace OnlineMinion.Application.PaymentSpec.Handlers;

[UsedImplicitly]
internal class GetPaymentSpecByIdReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetModelByIdReqHlr<GetPaymentSpecByIdReq, BasePaymentSpec, PaymentSpecResp>(dbContext)
{
    protected override PaymentSpecResp ToResponse(BasePaymentSpec entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        CurrencyCode = entity.CurrencyCode,
        Tags = entity.Tags,
    };
}
