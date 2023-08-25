using System.Globalization;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdatePaymentSpecReqHlr : BaseUpdateModelReqHlr<UpdatePaymentSpecReq>
{
    private readonly Uri _resource;

    public UpdatePaymentSpecReqHlr(ApiClientProvider api, ILogger<UpdatePaymentSpecReqHlr> logger)
        : base(api.Client, logger)
        => _resource = api.ApiV1PaymentSpecsUri;

    protected override string ModelName => "Payment Specification";

    protected override Uri BuildUri(UpdatePaymentSpecReq rq) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{_resource}/{rq.Id}"
        ),
        UriKind.RelativeOrAbsolute
    );
}
