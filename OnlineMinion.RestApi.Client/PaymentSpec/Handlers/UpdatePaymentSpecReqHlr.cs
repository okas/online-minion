using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecReqHlr(ApiClientProvider api, ILogger<UpdatePaymentSpecReqHlr> logger)
    : BaseUpdateModelReqHlr<UpdatePaymentSpecReq>(api.Client, api.ApiV1PaymentSpecsUri, logger)
{
    protected override string ModelName => "Payment Specification";
}
