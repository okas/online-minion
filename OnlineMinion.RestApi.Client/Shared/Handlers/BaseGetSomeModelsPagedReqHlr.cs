using System.Net.Http.Headers;
using System.Text.Json;
using MediatR;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseGetSomeModelsPagedReqHlr<TResponse>
    : IRequestHandler<BaseGetSomePagedReq<TResponse>, PagedResult<TResponse>>
{
    private readonly HttpClient _apiClient;
    protected BaseGetSomeModelsPagedReqHlr(HttpClient apiClient) => _apiClient = apiClient;

    public async Task<PagedResult<TResponse>> Handle(BaseGetSomePagedReq<TResponse> rq, CancellationToken ct)
    {
        var uri = BuildUrl(rq);

        var message = new HttpRequestMessage(HttpMethod.Get, uri);

        var responseMessage = await GetRequestResponse(message, ct).ConfigureAwait(false);

        responseMessage.EnsureSuccessStatusCode();

        var pagingMetaInfo = GetPagingInfo(rq, responseMessage.Headers);

        var modelsAsyncStream = await GetResultAsStreamAsync(responseMessage, ct).ConfigureAwait(false);

        return new(modelsAsyncStream, pagingMetaInfo);
    }

    protected abstract Uri BuildUrl(IQueryParams rq);

    /// <summary>
    ///     Intended to be called by implementer only to attach query string to the uri.
    /// </summary>
    protected static Uri AddQueryString(Uri uri, IQueryParams request)
    {
        var queryStringParams = new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase);

        if (!string.IsNullOrWhiteSpace(request.Filter))
        {
            queryStringParams[nameof(request.Filter)] = request.Filter;
        }

        if (!string.IsNullOrWhiteSpace(request.Sort))
        {
            queryStringParams[nameof(request.Sort)] = request.Sort;
        }

        queryStringParams[nameof(request.Page)] = request.Page;
        queryStringParams[nameof(request.Size)] = request.Size;

        return new(uri.AddQueryString(queryStringParams), UriKind.RelativeOrAbsolute);
    }

    private Task<HttpResponseMessage> GetRequestResponse(HttpRequestMessage message, CancellationToken ct) =>
        _apiClient.SendAsync(
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

    private static async ValueTask<IAsyncEnumerable<TResponse>> GetResultAsStreamAsync(
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

        // TODO: Compiler error about nullabiliti mismatch means ErrorOR should be implemented.
        return JsonSerializer.DeserializeAsyncEnumerable<TResponse>(stream, options, ct);
    }
}
