using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpec.CashAccountSpecHandlers;

[UsedImplicitly]
internal class GetCashAccountSpecByIdReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetModelByIdReqHlr<GetPaymentSpecByIdReq, PaymentSpecCash, PaymentSpecId, PaymentSpecResp>(dbContext)
{
    protected override PaymentSpecId CreateEntityId(GetPaymentSpecByIdReq rq) => new(rq.Id);

    protected override PaymentSpecResp ToResponse(PaymentSpecCash entity) => new()
    {
        Id = entity.Id.Value,
        Name = entity.Name,
        CurrencyCode = entity.CurrencyCode,
        Tags = entity.Tags,
    };
}
