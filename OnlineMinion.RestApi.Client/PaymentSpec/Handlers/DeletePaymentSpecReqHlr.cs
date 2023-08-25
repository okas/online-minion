using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class DeletePaymentSpecReqHlr : BaseDeleteModelReqHlr<DeletePaymentSpecReq>
{
    private readonly Uri _resource;

    public DeletePaymentSpecReqHlr(ApiClientProvider api) : base(api.Client) =>
        _resource = api.ApiV1PaymentSpecsUri;

    protected override Uri BuildUrl(DeletePaymentSpecReq request) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{_resource}/{request.Id}"
        ),
        UriKind.RelativeOrAbsolute
    );
}
