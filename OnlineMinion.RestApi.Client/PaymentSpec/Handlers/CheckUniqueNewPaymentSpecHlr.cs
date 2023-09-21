using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Connectivity;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewPaymentSpecHlr(ApiClientProvider api)
    : BaseCheckUniqueReqHlr<CheckPaymentSpecUniqueNewReq>(api.Client)
{
    public override Uri BuildUri(CheckPaymentSpecUniqueNewReq rq) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{api.ApiV1PaymentSpecsUri}/validate-available-name/{rq.MemberValue}"
        ),
        UriKind.RelativeOrAbsolute
    );
}
