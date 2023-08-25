using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class DeleteAccountSpecReqHlr : BaseDeleteModelReqHlr<DeleteAccountSpecReq>
{
    private readonly Uri _resource;

    public DeleteAccountSpecReqHlr(ApiClientProvider apiClient) : base(apiClient.Client) =>
        _resource = apiClient.ApiV1AccountSpecsUri;

    protected override Uri BuildUrl(DeleteAccountSpecReq request) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{_resource}/{request.Id}"
        ),
        UriKind.RelativeOrAbsolute
    );
}
