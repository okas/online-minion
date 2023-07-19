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

internal sealed class GetPagedAccountSpecsReqHlr : IRequestHandler<GetAccountSpecsReq, PagedResult<AccountSpecResp>>
{
    private readonly ApiClientProvider _api;
    public GetPagedAccountSpecsReqHlr(ApiClientProvider api) => _api = api;

    public async Task<PagedResult<AccountSpecResp>> Handle(GetAccountSpecsReq request, CancellationToken ct)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, CreateUri(request));

        var httpResponseMessage = await GetRequestResponse(message, ct).ConfigureAwait(false);

        httpResponseMessage.EnsureSuccessStatusCode();

        var pagingMetaInfo = GetPagingInfo(request, httpResponseMessage.Headers);

        var modelsAsyncStream = await GetResultAsStreamAsync(httpResponseMessage, ct);

        return new(modelsAsyncStream, pagingMetaInfo);
    }

    private string CreateUri(GetAccountSpecsReq request)
    {
        var qsParams = new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase);
        if (!string.IsNullOrWhiteSpace(request.Filter))
        {
            qsParams[nameof(request.Filter)] = request.Filter;
        }

        if (!string.IsNullOrWhiteSpace(request.Sort))
        {
            qsParams[nameof(request.Sort)] = request.Sort;
        }

        qsParams[nameof(request.Page)] = request.Page;
        qsParams[nameof(request.Size)] = request.Size;

        return _api.ApiV1AccountSpecsUri.AddQueryString(qsParams);
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

    private static PagingMetaInfo GetPagingInfo(IPagingInfo request, HttpResponseHeaders headers)
    {
        var size = headers.GetHeaderFirstValue<int>(CustomHeaderNames.PagingSize);
        var totalItems = headers.GetHeaderFirstValue<int>(CustomHeaderNames.PagingRows);

        return new(totalItems, size, request.Page);
    }

    private static async Task<IAsyncEnumerable<AccountSpecResp>> GetResultAsStreamAsync(
        HttpResponseMessage httpResponseMessage,
        CancellationToken   ct
    )
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultBufferSize = 16,
        };

        var stream = await httpResponseMessage.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);

        return JsonSerializer.DeserializeAsyncEnumerable<AccountSpecResp>(stream, options, ct);
    }
}
