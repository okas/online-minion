using System.Net.Http.Headers;
using System.Text.Json;
using ErrorOr;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BasePagedGetSomeModelsReqHlr<TResponse>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<BaseGetSomeModelsPagedReq<TResponse>, PagedResult<TResponse>>
{
    public virtual Uri BuildUri(BaseGetSomeModelsPagedReq<TResponse> rq) => AddQueryString(resource, rq);

    public async Task<ErrorOr<PagedResult<TResponse>>> Handle(
        BaseGetSomeModelsPagedReq<TResponse> rq,
        CancellationToken                    ct
    )
    {
        var uri = BuildUri(rq);

        var message = new HttpRequestMessage(HttpMethod.Get, uri);

        var responseMessage = await GetRequestResponse(message, ct).ConfigureAwait(false);

        return await HandleResponse(responseMessage, rq, ct).ConfigureAwait(false);
    }

    private Task<HttpResponseMessage> GetRequestResponse(HttpRequestMessage message, CancellationToken ct) =>
        apiClient.SendAsync(
            /* Important: *browser streaming* and *http completion* are requirements to get streaming behavior working.
             * NB! It is expected to be configured as DelegatingHandler for HttpClientFactory!
             * This way current assembly do not hold references to WebAssembly specific dependencies. */
            // message.SetBrowserResponseStreamingEnabled(true),
            message,
            HttpCompletionOption.ResponseHeadersRead,
            ct
        );

    private static async ValueTask<ErrorOr<PagedResult<TResponse>>> HandleResponse(
        HttpResponseMessage responseMessage,
        IPagingInfo         rq,
        CancellationToken   ct
    )
    {
        if (!responseMessage.IsSuccessStatusCode)
        {
            return Error.Unexpected();
        }

        var pagingMetaInfo = GetPagingInfo(rq, responseMessage.Headers);

        var modelsAsyncStream = await GetResultAsStreamAsync(responseMessage, ct).ConfigureAwait(false);

        return new PagedResult<TResponse>(modelsAsyncStream, pagingMetaInfo);
    }

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

    protected virtual Uri AddQueryString(Uri uri, IQueryParams rq)
    {
        var queryStringParams = new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase);

        if (!string.IsNullOrWhiteSpace(rq.Filter))
        {
            queryStringParams[nameof(rq.Filter)] = rq.Filter;
        }

        if (!string.IsNullOrWhiteSpace(rq.Sort))
        {
            queryStringParams[nameof(rq.Sort)] = rq.Sort;
        }

        queryStringParams[nameof(rq.Page)] = rq.Page;
        queryStringParams[nameof(rq.Size)] = rq.Size;

        return new(uri.AddQueryString(queryStringParams), UriKind.RelativeOrAbsolute);
    }
}
