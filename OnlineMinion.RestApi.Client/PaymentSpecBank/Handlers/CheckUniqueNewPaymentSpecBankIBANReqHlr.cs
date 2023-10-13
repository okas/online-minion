using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpecBank.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueNewPaymentSpecBankIBANReqHlr(ApiProvider api)
    : BaseCheckUniqueReqHlr<CheckUniqueNewPaymentSpecBankIBANReq>(api.Client)
{
    public override Uri BuildUri(CheckUniqueNewPaymentSpecBankIBANReq rq) => new(
        $"{ApiProvider.ApiPaymentSpecsBankUri}/validate-available-iban/{rq.MemberValue}",
        UriKind.RelativeOrAbsolute
    );
}
