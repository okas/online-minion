using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CreatePaymentSpecReqHlr(ApiProvider api, ILogger<CreatePaymentSpecReqHlr> logger)
    : BaseCreateModelReqHlr<CreatePaymentSpecReq>(api.Client, ApiProvider.ApiPaymentSpecsUri, logger)
{
    protected override string ModelName => "Payment Specification";
}
