using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Application.Contracts.PaymentSpecCash.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpecCash.Handlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecCashReqHlr(ApiProvider api, ILogger<UpdatePaymentSpecCashReqHlr> logger)
    : BaseUpdateModelReqHlr<UpdatePaymentSpecCashReq>(api.Client, ApiProvider.ApiPaymentSpecsUri, logger)
{
    protected override string ModelName => "Payment Specification";

    public override Uri BuildUri(UpdatePaymentSpecCashReq rq) => new(
        $"{ApiProvider.ApiPaymentSpecsUri}/cash/{rq.Id}",
        UriKind.RelativeOrAbsolute
    );
}
