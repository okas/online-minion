using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecDescriptorsReqHlr(ApiClientProvider api)
    : GetSomeModelDescriptorsReqHlr<PaymentSpecDescriptorResp>(api.Client, api.ApiV1PaymentSpecsUri);
