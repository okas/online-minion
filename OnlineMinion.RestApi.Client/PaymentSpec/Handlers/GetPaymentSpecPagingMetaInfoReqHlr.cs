using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecPagingMetaInfoReqHlr(ApiClientProvider api)
    : BaseGetModelPagingMetaInfoReqHlr<GetPaymentSpecPagingMetaInfoReq>(api.Client, api.ApiV1PaymentSpecsUri);
