using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.PaymentSpecShared.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewPaymentSpecHlr(ApiProvider api)
    : BaseCheckUniqueReqHlr<CheckPaymentSpecUniqueNewReq>(api.Client)
{
    public override Uri BuildUri(CheckPaymentSpecUniqueNewReq rq) => new(
        $"{ApiPaymentSpecsUri}/validate-available-name/{rq.MemberValue}",
        UriKind.RelativeOrAbsolute
    );
}
