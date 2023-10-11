using System.Net.Http.Json;
using JetBrains.Annotations;
using OnlineMinion.Application.Contracts;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Requests;
using OnlineMinion.Application.Contracts.PaymentSpecShared.Responses;
using OnlineMinion.RestApi.Client.Api;
using OnlineMinion.RestApi.Client.Shared.Handlers;
using static OnlineMinion.RestApi.Client.Api.ApiProvider;

namespace OnlineMinion.RestApi.Client.PaymentSpecShared.Handlers;

[UsedImplicitly]
internal sealed class GetByIdPaymentSpecReqHlr(ApiProvider api)
    : BaseGetModelByIdReqHlr<GetByIdPaymentSpecReq, BasePaymentSpecResp>(api.Client, ApiPaymentSpecsUri)
{
    protected override async ValueTask<BasePaymentSpecResp?> ToResponse(
        HttpResponseMessage response,
        CancellationToken   ct
    )
    {
        var concreteType = response.Content.Headers.ContentType?.MediaType switch
        {
            CustomVendorContentTypes.PaymentSpecCashJson => typeof(PaymentSpecCashResp),
            CustomVendorContentTypes.PaymentSpecBankJson => typeof(PaymentSpecBankResp),
            CustomVendorContentTypes.PaymentSpecCryptoJson => typeof(PaymentSpecCryptoResp),
            _ => typeof(void),
        };

        if (concreteType == typeof(void))
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync(concreteType, ct).ConfigureAwait(false) as BasePaymentSpecResp;
    }
}
