using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueExistingAccountSpecReqHlr
    (ApiClientProvider api) : BaseCheckUniqueReqHlr<CheckAccountSpecUniqueExistingReq>(api.Client)
{
    public override Uri BuildUri(CheckAccountSpecUniqueExistingReq rq) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{api.ApiV1AccountSpecsUri}/validate-available-name/{rq.Name}/except-id/{rq.ExceptId}"
        ),
        UriKind.RelativeOrAbsolute
    );
}
