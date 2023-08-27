using ErrorOr;
using OnlineMinion.Common.Utilities;
using OnlineMinion.Contracts;
using OnlineMinion.Contracts.Shared.Requests;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseGetModelPagingMetaInfoReqHlr<TRequest>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<TRequest, PagingMetaInfo>
    where TRequest : IGetPagingInfoRequest
{
    public virtual Uri BuildUri(TRequest rq) => AddQueryString(resource, rq);

    public async Task<ErrorOr<PagingMetaInfo>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        var message = new HttpRequestMessage(HttpMethod.Head, uri);

        using var responseMessage = await apiClient
            .SendAsync(message, HttpCompletionOption.ResponseHeadersRead, ct)
            .ConfigureAwait(false);

        return HandleResponse(responseMessage);
    }

    private static ErrorOr<PagingMetaInfo> HandleResponse(HttpResponseMessage response) =>
        response.IsSuccessStatusCode switch
        {
            false => Error.Unexpected(),
            _ => response.GetPagingMetaInfo(),
        };


    protected virtual Uri AddQueryString(Uri uri, IGetPagingInfoRequest rq) => new(
        uri.AddQueryString(
            new Dictionary<string, object?>(StringComparer.InvariantCultureIgnoreCase)
                { [nameof(rq.PageSize)] = rq.PageSize, }
        ),
        UriKind.RelativeOrAbsolute
    );
}
