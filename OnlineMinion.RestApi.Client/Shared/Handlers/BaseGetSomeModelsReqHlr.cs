using ErrorOr;
using MediatR;
using static OnlineMinion.RestApi.Client.Shared.Handlers.ICollectionRequestResponseStreaming;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseGetSomeModelsReqHlr<TRequest, TResponse>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<TRequest, IAsyncEnumerable<TResponse>>,
        ICollectionRequestResponseStreaming
    where TRequest : IRequest<ErrorOr<IAsyncEnumerable<TResponse>>>
{
    public async Task<ErrorOr<IAsyncEnumerable<TResponse>>> Handle(TRequest rq, CancellationToken ct)
    {
        var uri = BuildUri(rq);

        return await GetApiResponse<TResponse>(apiClient, uri, ct).ConfigureAwait(false);
    }

    public virtual Uri BuildUri(TRequest _) => resource;
}
