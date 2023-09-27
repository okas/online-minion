using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPagedPaymentSpecsReqHlr(ApiProvider api)
    : BaseGetSomeModelsPagedReqHlr<PaymentSpecResp>(api.Client, ApiProvider.ApiPaymentSpecsUri);
