using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewAccountSpecReqHlr(ApiClientProvider api)
    : BaseCheckUniqueReqHlr<CheckAccountSpecUniqueNewReq>(api.Client)
{
    public override Uri BuildUri(CheckAccountSpecUniqueNewReq rq) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{api.ApiV1AccountSpecsUri}/validate-available-name/{rq.MemberValue}"
        ),
        UriKind.RelativeOrAbsolute
    );
}
