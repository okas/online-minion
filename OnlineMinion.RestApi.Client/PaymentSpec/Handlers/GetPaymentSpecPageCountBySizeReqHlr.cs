using JetBrains.Annotations;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecPageCountBySizeReqHlr(ApiClientProvider api)
    : BaseGetModelPageCountReqHlr<GetPaymentSpecPageCountBySizeReq>(api.Client, api.ApiV1PaymentSpecsUri);
