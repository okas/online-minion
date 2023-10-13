using JetBrains.Annotations;
using OnlineMinion.Application.Contracts.PaymentSpecBank.Requests;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;

namespace OnlineMinion.RestApi.Client.PaymentSpecBank.Handlers;

[UsedImplicitly]
internal sealed class CheckUniqueExistingPaymentSpecBankIBANReqHlr(ApiProvider api)
    : BaseCheckUniqueReqHlr<CheckUniqueExistingPaymentSpecBankIBANReq>(api.Client)
{
    public override Uri BuildUri(CheckUniqueExistingPaymentSpecBankIBANReq rq) => new(
        $"{ApiProvider.ApiPaymentSpecsBankUri}/validate-available-iban/{rq.MemberValue}/except-id/{rq.OwnId}",
        UriKind.RelativeOrAbsolute
    );
}
