using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CreatePaymentSpecReqHlr : BaseCreateModelReqHlr<CreatePaymentSpecReq>
{
    private readonly Uri _resource;

    public CreatePaymentSpecReqHlr(ApiClientProvider api, ILogger<CreatePaymentSpecReqHlr> logger)
        : base(api.Client, logger) =>
        _resource = api.ApiV1PaymentSpecsUri;

    protected override string ModelName => "Payment Specification";

    protected override Uri BuildUri() => _resource;
}
