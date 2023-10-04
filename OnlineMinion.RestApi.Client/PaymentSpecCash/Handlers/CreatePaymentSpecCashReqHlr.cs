using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpecCash.Handlers;

[UsedImplicitly]
internal sealed class CreatePaymentSpecCashReqHlr(ApiProvider api, ILogger<CreatePaymentSpecCashReqHlr> logger)
    : BaseCreateModelReqHlr<CreatePaymentSpecCashReq>(api.Client, ApiProvider.ApiPaymentSpecsUri, logger)
{
    protected override string ModelName => "Payment Specification";

    public override Uri BuildUri(CreatePaymentSpecCashReq rq) => new(
        $"{ApiProvider.ApiPaymentSpecsUri}/cash",
        UriKind.RelativeOrAbsolute
    );
}
