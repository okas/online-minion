using ErrorOr;
using MediatR;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseGetModelPageCountReqHlr<TRequest> : IRequestHandler<TRequest, ErrorOr<int>>
    where TRequest : IGetPagingInfoReq
{
    private readonly HttpClient _apiClient;
    protected BaseGetModelPageCountReqHlr(HttpClient apiClient) => _apiClient = apiClient;

    public async Task<ErrorOr<int>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUrl(rq);

        var message = new HttpRequestMessage(HttpMethod.Head, uri);

        using var result = await _apiClient
            .SendAsync(message, HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        return result.IsSuccessStatusCode switch
        {
            false => Error.Unexpected(),
            _ => result.Headers.GetHeaderFirstValue<int>(CustomHeaderNames.PagingPages) is var count
                ? count
                : Error.Failure(description: "Paging info not found in header."),
        };
    }

    protected abstract Uri BuildUrl(TRequest rq);

    /// <summary>
    ///     Intended to be called by implementer only to attach query string to the uri.
    /// </summary>
    protected static Uri AddQueryString(Uri uri, TRequest rq) => new(
        uri.AddQueryString(
            new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase)
                { [nameof(rq.PageSize)] = rq.PageSize, }
        ),
        UriKind.RelativeOrAbsolute
    );
}
