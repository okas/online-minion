using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecSharedHandlers;

[UsedImplicitly]
internal class GetByIdPaymentSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetModelByIdReqHlr<GetByIdPaymentSpecReq, PaymentSpecCash, PaymentSpecId, PaymentSpecResp>(dbContext)
{
    protected override PaymentSpecId CreateEntityId(GetByIdPaymentSpecReq rq) => new(rq.Id);

    protected override PaymentSpecResp ToResponse(PaymentSpecCash entity) => new()
    {
        Id = entity.Id.Value,
        Name = entity.Name,
        CurrencyCode = entity.CurrencyCode,
        Tags = entity.Tags,
    };
}
