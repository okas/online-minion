using ErrorOr;
using OnlineMinion.Application.Contracts.Shared.Requests;
using OnlineMinion.Application.Contracts.Shared.Responses;
using static OnlineMinion.RestApi.Client.Shared.Handlers.IPagedRequestResultStreaming;

namespace OnlineMinion.RestApi.Client.Shared.Handlers;

internal abstract class BaseGetSomeModelsPagedReqHlr<TResponse>(HttpClient apiClient, Uri resource)
    : IApiClientRequestHandler<GetSomeModelsPagedReq<TResponse>, PagedStreamResult<TResponse>>,
        IPagedRequestResultStreaming
{
    public async Task<ErrorOr<PagedStreamResult<TResponse>>> Handle(
        GetSomeModelsPagedReq<TResponse> rq,
        CancellationToken                ct
    )
    {
        var uri = BuildUri(rq);

        return await GetApiResponse<TResponse>(apiClient, uri, rq, ct).ConfigureAwait(false);
    }

    public virtual Uri BuildUri(GetSomeModelsPagedReq<TResponse> rq) => AddQueryString(resource, rq);
}
