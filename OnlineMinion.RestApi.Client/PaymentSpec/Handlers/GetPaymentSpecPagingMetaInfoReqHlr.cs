using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecPagingMetaInfoReqHlr(ApiProvider api)
    : BaseGetModelPagingMetaInfoReqHlr<GetPaymentSpecPagingMetaInfoReq>(api.Client, api.ApiPaymentSpecsUri);
