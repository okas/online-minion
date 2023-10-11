using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCrypto.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecCryptoHandlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecCryptoReqHlr(IOnlineMinionDbContext dbContext)
    : BaseUpdateModelReqHlr<UpdatePaymentSpecCryptoReq, CryptoExchangeAccountSpec, PaymentSpecId>(dbContext)
{
    protected override PaymentSpecId CreateEntityId(UpdatePaymentSpecCryptoReq rq) => new(rq.Id);

    protected override void UpdateEntity(CryptoExchangeAccountSpec entity, UpdatePaymentSpecCryptoReq rq) =>
        entity.Update(rq.ExchangeName, rq.IsFiat, rq.Name, rq.Tags);
}
