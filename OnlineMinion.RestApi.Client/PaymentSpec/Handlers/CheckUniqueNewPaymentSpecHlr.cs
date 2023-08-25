using System.Globalization;
using JetBrains.Annotations;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.RestApi.Client.Common.Handlers;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewPaymentSpecHlr : BaseCheckUniqueReqHlr<CheckPaymentSpecUniqueNewReq>
{
    public CheckUniqueNewPaymentSpecHlr(ApiClientProvider api) : base(api) { }

    protected override Uri BuildUrl(CheckPaymentSpecUniqueNewReq rq) => new(
        string.Create(
            CultureInfo.InvariantCulture,
            $"{Api.ApiV1PaymentSpecsUri}/validate-available-name/{rq.Name}"
        ),
        UriKind.RelativeOrAbsolute
    );
}
