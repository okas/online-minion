using System.Net.Http.Json;
using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Contracts.PaymentSpec.Requests;
using OnlineMinion.Contracts.PaymentSpec.Responses;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecByIdReqHlr : IRequestHandler<GetPaymentSpecByIdReq, PaymentSpecResp?>
{
    private readonly ApiClientProvider _api;
    public GetPaymentSpecByIdReqHlr(ApiClientProvider api) => _api = api;

    public async Task<PaymentSpecResp?> Handle(GetPaymentSpecByIdReq rq, CancellationToken cancellationToken)
    {
        var uri = $"{_api.ApiV1PaymentSpecsUri}/{rq.Id}";

        return await _api.Client.GetFromJsonAsync<PaymentSpecResp?>(uri, cancellationToken).ConfigureAwait(false);
    }
}
