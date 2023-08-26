using ErrorOr;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseGetModelPageCountReqHlr<TRequest>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<TRequest, int>
    where TRequest : IGetPagingInfoReq
{
    public async Task<ErrorOr<int>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        var message = new HttpRequestMessage(HttpMethod.Head, uri);

        using var responseMessage = await apiClient
            .SendAsync(message, HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        return HandleResponse(responseMessage);
    }

    public virtual Uri BuildUri(TRequest rq) => AddQueryString(resource, rq);

    public ErrorOr<int> HandleResponse(HttpResponseMessage response) => response.IsSuccessStatusCode switch
    {
        false => Error.Unexpected(),
        _ => response.Headers.GetHeaderFirstValue<int>(CustomHeaderNames.PagingPages) is var count
            ? count
            : Error.Failure(description: "Paging info not found in header."),
    };

    protected virtual Uri AddQueryString(Uri uri, TRequest rq) => new(
        uri.AddQueryString(
            new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase)
                { [nameof(rq.PageSize)] = rq.PageSize, }
        ),
        UriKind.RelativeOrAbsolute
    );
}
