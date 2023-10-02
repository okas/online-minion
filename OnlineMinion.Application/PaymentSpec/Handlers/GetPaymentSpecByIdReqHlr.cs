using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpec.Requests;
using OnlineMinion.Application.Contracts.PaymentSpec.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpec.Handlers;

[UsedImplicitly]
internal class GetPaymentSpecByIdReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetModelByIdReqHlr<GetPaymentSpecByIdReq, CashAccountSpec, BasePaymentSpecId, PaymentSpecResp>(dbContext)
{
    protected override BasePaymentSpecId CreateEntityId(GetPaymentSpecByIdReq rq) => new(rq.Id);

    protected override PaymentSpecResp ToResponse(CashAccountSpec entity) => new()
    {
        Id = entity.Id.Value,
        Name = entity.Name,
        CurrencyCode = entity.CurrencyCode,
        Tags = entity.Tags,
    };
}
