using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCrypto.Requests;
using OnlineMinion.Application.Shared.Handlers;
using OnlineMinion.Domain.PaymentSpecs;

namespace OnlineMinion.Application.PaymentSpecCryptoHandlers;

[UsedImplicitly]
internal sealed class CreatePaymentSpecCryptoReqHlr(IOnlineMinionDbContext dbContext)
    : BaseCreateModelReqHlr<CreatePaymentSpecCryptoReq, CryptoExchangeAccountSpec>(dbContext)
{
    protected override CryptoExchangeAccountSpec ToEntity(CreatePaymentSpecCryptoReq rq) => new(
        rq.ExchangeName,
        rq.IsFiat,
        rq.Name,
        rq.CurrencyCode,
        rq.Tags
    );
}