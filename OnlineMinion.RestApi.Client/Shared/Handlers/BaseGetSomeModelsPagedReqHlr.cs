using ErrorOr;
using OnlineMinion.Contracts.Shared.Requests;
using OnlineMinion.Contracts.Shared.Responses;

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

        return await IPagedRequestResultStreaming.GetApiResponse<TResponse>(apiClient, uri, rq, ct);
    }

    public virtual Uri BuildUri(GetSomeModelsPagedReq<TResponse> rq) =>
        IPagedRequestResultStreaming.AddQueryString(resource, rq);
}
