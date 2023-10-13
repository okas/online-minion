using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecCrypto.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpecCrypto.Handlers;

[UsedImplicitly]
internal sealed class CreatePaymentSpecCryptoReqHlr(ApiProvider api)
    : BaseCreateModelReqHlr<CreatePaymentSpecCryptoReq>(api.Client, ApiProvider.ApiPaymentSpecsCryptoUri);
