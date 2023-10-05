using ErrorOr;
using OnlineMinion.Application.Contracts;
using OnlineMinion.Application.Contracts.Shared.Requests;
using static OnlineMinion.RestApi.Client.Shared.Handlers.ICollectionRequestResponseStreaming;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class GetSomeModelDescriptorsReqHlr<TResponse>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<GetSomeModelDescriptorsReq<TResponse>, IAsyncEnumerable<TResponse>>,
        ICollectionRequestResponseStreaming
    where TResponse : IHasId
{
    public async Task<ErrorOr<IAsyncEnumerable<TResponse>>> Handle(
        GetSomeModelDescriptorsReq<TResponse> rq,
        CancellationToken                     ct
    )
    {
        var uri = BuildUri(rq);

        return await GetApiResponse<TResponse>(apiClient, uri, ct).ConfigureAwait(false);
    }

    public virtual Uri BuildUri(GetSomeModelDescriptorsReq<TResponse> rq) => new(
        $"{resource}/descriptors",
        UriKind.RelativeOrAbsolute
    );
}
