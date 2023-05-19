using System.Net.Http.Headers;
using System.Text.Json;
using MediatR;
using OnlineMinion.Common.Utilities.Extensions;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.AppMessaging.Requests;
using OnlineMinion.Contracts.HttpHeaders;
using OnlineMinion.Contracts.Responses;
using OnlineMinion.RestApi.Client.Infrastructure;

namespace OnlineMinion.RestApi.Client.Handlers;

internal sealed class GetPagedAccountSpecsReqHlr : IRequestHandler<GetAccountSpecsReq, BasePagedResult<AccountSpecResp>>
{
    private readonly ApiClientProvider _api;
    public GetPagedAccountSpecsReqHlr(ApiClientProvider api) => _api = api;

    public async Task<BasePagedResult<AccountSpecResp>> Handle(GetAccountSpecsReq request, CancellationToken ct)
    {
        var message = CreateGetMessage(request);

        var httpResponseMessage = await GetRequestResponse(message, ct).ConfigureAwait(false);

        httpResponseMessage.EnsureSuccessStatusCode();

        var pagingMetaInfo = GetPagingInfo(request, httpResponseMessage.Headers);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultBufferSize = 16,
        };
        var stream = await httpResponseMessage.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
        var modelsAsyncStream = JsonSerializer.DeserializeAsyncEnumerable<AccountSpecResp>(stream, options, ct);

        return new(modelsAsyncStream, pagingMetaInfo);
    }

    private HttpRequestMessage CreateGetMessage(GetAccountSpecsReq request)
    {
        var uri = _api.ApiV1AccountSpecsUri.AddQueryString(
            new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase)
            {
                [nameof(request.Page)] = request.Page,
                [nameof(request.PageSize)] = request.PageSize,
            }
        );

        return new(HttpMethod.Get, uri);
    }

    private Task<HttpResponseMessage> GetRequestResponse(HttpRequestMessage message, CancellationToken ct) =>
        _api.Client.SendAsync(
            /* Important: *browser streaming* and *http completion* are requirements to get streaming behavior working.
             * NB! It is expected to be configured as DelegatingHandler for HttpClientFactory!
             * This way current assembly do not hold references to WebAssembly specific dependencies. */
            // message.SetBrowserResponseStreamingEnabled(true),
            message,
            HttpCompletionOption.ResponseHeadersRead,
            ct
        );

    private static PagingMetaInfo GetPagingInfo(GetAccountSpecsReq request, HttpResponseHeaders headers)
    {
        var size = headers.GetHeaderFirstValue<int>(CustomHeaderNames.PagingSize);
        var totalItems = headers.GetHeaderFirstValue<int>(CustomHeaderNames.PagingTotalItems);

        return new(totalItems, size, request.Page);
    }
}
