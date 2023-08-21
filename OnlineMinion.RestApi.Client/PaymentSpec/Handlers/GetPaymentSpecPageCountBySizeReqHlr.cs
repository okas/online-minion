using JetBrains.Annotations;
using MediatR;
using OnlineMinion.Common.Utilities.Extensions;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.RestApi.Client.Infrastructure;
using OnlineMinion.RestApi.Client.PaymentSpec.Requests;

namespace OnlineMinion.RestApi.Client.PaymentSpec.Handlers;

[UsedImplicitly]
internal sealed class GetPaymentSpecPageCountBySizeReqHlr : IRequestHandler<GetPaymentSpecPageCountBySizeReq, int?>
{
    private readonly ApiClientProvider _api;
    public GetPaymentSpecPageCountBySizeReqHlr(ApiClientProvider api) => _api = api;

    public async Task<int?> Handle(GetPaymentSpecPageCountBySizeReq rq, CancellationToken ct)
    {
        var uri = _api.ApiV1PaymentSpecsUri.AddQueryString(
            new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase)
                { [nameof(rq.PageSize)] = rq.PageSize, }
        );

        var message = new HttpRequestMessage(HttpMethod.Head, uri);

        using var result = await _api.Client
            .SendAsync(message, HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        return result.Headers.GetHeaderFirstValue<int?>(CustomHeaderNames.PagingPages);
    }
}
