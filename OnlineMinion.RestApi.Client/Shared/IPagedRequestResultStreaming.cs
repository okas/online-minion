using ErrorOr;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Responses;

namespace OnlineMinion.RestApi.Client.Shared;

/// <inheritdoc cref="IRequestResponseStreaming" />
internal interface IPagedRequestResultStreaming : IRequestResponseStreaming
{
    /// <inheritdoc cref="ICollectionRequestResponseStreaming.GetApiResponse{TResponse}" />
    /// <param name="rq">Instance to obtain paging and filtering related query parameters.</param>
    protected static async ValueTask<ErrorOr<PagedStreamResult<TResponse>>> GetApiResponse<TResponse>(
        HttpClient        apiClient,
        Uri               uri,
        IPagingInfo       rq,
        CancellationToken ct
    )
    {
        var responseMessage = await GetHttpRequestResponse(apiClient, uri, ct).ConfigureAwait(false);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return ToStreamResponseApiError(responseMessage);
        }

        var modelsAsyncStream = await GetResultAsStreamAsync<TResponse>(responseMessage, ct);

        var pagingMetaInfo = responseMessage.GetPagingMetaInfo(rq.Page);

        return new PagedStreamResult<TResponse>(modelsAsyncStream, pagingMetaInfo.Value);
    }

    /// <summary>
    ///     Appends query string parameters to the <paramref name="uri" /> based on the <paramref name="rq" /> instance.
    /// </summary>
    /// <param name="uri">URI to append any optional parameters.</param>
    /// <param name="rq">Paging and filtering parameters source.</param>
    /// <returns></returns>
    protected static Uri AddQueryString(Uri uri, IQueryParams rq)
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
