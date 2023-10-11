using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecSharedHandlers;

[UsedImplicitly]
internal class GetByIdPaymentSpecReqHlr(IOnlineMinionDbContext dbContext)
    : BaseGetModelByIdReqHlr<GetByIdPaymentSpecReq, BasePaymentSpecData, PaymentSpecId, BasePaymentSpecResp>(dbContext)
{
    protected override PaymentSpecId CreateEntityId(GetByIdPaymentSpecReq rq) => new(rq.Id);

    protected override BasePaymentSpecResp ToResponse(BasePaymentSpecData entity)
    {
        BasePaymentSpecResp response = entity switch
        {
            PaymentSpecCash e => new PaymentSpecCashResp(
                e.Id.Value,
                e.Name,
                e.CurrencyCode,
                e.Tags
            ),

            PaymentSpecBank e => new PaymentSpecBankResp(
                e.Id.Value,
                e.Name,
                e.CurrencyCode,
                e.Tags,
                e.IBAN
            ),

            PaymentSpecCrypto e => new PaymentSpecCryptoResp(
                e.Id.Value,
                e.Name,
                e.CurrencyCode,
                e.Tags,
                e.ExchangeName,
                e.IsFiat
            ),

            _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, null),
        };

        return response;
    }
}
