using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.PaymentSpecShared.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueExistingPaymentSpecReqHlr(ApiProvider api)
    : BaseCheckUniqueReqHlr<CheckUniqueExistingPaymentSpecNameReq>(api.Client)
{
    public override Uri BuildUri(CheckUniqueExistingPaymentSpecNameReq rq) => new(
        $"{ApiPaymentSpecsUri}/validate-available-name/{rq.MemberValue}/except-id/{rq.OwnId}",
        UriKind.RelativeOrAbsolute
    );
}
