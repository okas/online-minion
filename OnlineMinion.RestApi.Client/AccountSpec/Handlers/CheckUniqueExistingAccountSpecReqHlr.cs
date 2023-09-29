using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueExistingAccountSpecReqHlr(ApiProvider api)
    : BaseCheckUniqueReqHlr<CheckAccountSpecUniqueExistingReq>(api.Client)
{
    public override Uri BuildUri(CheckAccountSpecUniqueExistingReq rq) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{ApiProvider.ApiAccountSpecsUri}/validate-available-name/{rq.MemberValue}/except-id/{rq.OwnId}"
        ),
        UriKind.RelativeOrAbsolute
    );
}
