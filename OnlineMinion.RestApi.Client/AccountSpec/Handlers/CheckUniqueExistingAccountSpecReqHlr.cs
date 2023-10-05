using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.AccountSpec.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.AccountSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueExistingAccountSpecReqHlr(ApiProvider api)
    : BaseCheckUniqueReqHlr<CheckAccountSpecUniqueExistingReq>(api.Client)
{
    public override Uri BuildUri(CheckAccountSpecUniqueExistingReq rq) => new(
        $"{ApiAccountSpecsUri}/validate-available-name/{rq.MemberValue}/except-id/{rq.OwnId}",
        UriKind.RelativeOrAbsolute
    );
}
