using ErrorOr;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseGetSomeModelsPagedReqHlr<TResponse>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<GetSomeModelsPagedReq<TResponse>, PagedStreamResult<TResponse>>,
        IRequestResponseStreaming<TResponse>
{
    public async Task<ErrorOr<PagedStreamResult<TResponse>>> Handle(
        GetSomeModelsPagedReq<TResponse> rq,
        CancellationToken                ct
    )
    {
        var uri = BuildUri(rq);

        var message = new HttpRequestMessage(HttpMethod.Get, uri);

        var responseMessage = await IRequestResponseStreaming<TResponse>
            .GetRequestResponse(apiClient, message, ct)
            .ConfigureAwait(false);

        return await HandleResponse(responseMessage, rq, ct).ConfigureAwait(false);
    }

    public virtual Uri BuildUri(GetSomeModelsPagedReq<TResponse> rq) => AddQueryString(resource, rq);

    private static async Task<ErrorOr<PagedStreamResult<TResponse>>> HandleResponse(
        HttpResponseMessage responseMessage,
        IPagingInfo         rq,
        CancellationToken   ct
    )
    {
        if (!responseMessage.IsSuccessStatusCode)
        {
            return Error.Unexpected(description: "Unexpected response from server");
        }

        var pagingMetaInfo = responseMessage.GetPagingMetaInfo(rq.Page);

        var modelsAsyncStream = await IRequestResponseStreaming<TResponse>
            .GetResultAsStreamAsync(responseMessage, ct)
            .ConfigureAwait(false);

        return new PagedStreamResult<TResponse>(modelsAsyncStream, pagingMetaInfo.Value);
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
