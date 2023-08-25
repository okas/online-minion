using System.Globalization;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class UpdateAccountSpecReqHlr : BaseUpdateModelReqHlr<UpdateAccountSpecReq>
{
    private readonly Uri _resource;

    public UpdateAccountSpecReqHlr(ApiClientProvider api, ILogger<UpdateAccountSpecReqHlr> logger)
        : base(api.Client, logger) =>
        _resource = api.ApiV1AccountSpecsUri;

    protected override string ModelName => "Account Specification";

    protected override Uri BuildUri(UpdateAccountSpecReq rq) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{_resource}/{rq.Id}"
        ),
        UriKind.RelativeOrAbsolute
    );
}
