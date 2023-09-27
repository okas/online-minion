using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueExistingPaymentSpecReqHlr(ApiProvider api)
    : BaseCheckUniqueReqHlr<CheckPaymentSpecUniqueExistingReq>(api.Client)
{
    public override Uri BuildUri(CheckPaymentSpecUniqueExistingReq rq) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{ApiProvider.ApiPaymentSpecsUri}/validate-available-name/{rq.MemberValue}/except-id/{rq.OwnId}"
        ),
        UriKind.RelativeOrAbsolute
    );
}
